using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace LMS.Web {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodAttribute : Attribute {
        public MethodAttribute() : this(true) { }
        public MethodAttribute(bool anonymous) {
            this.Anonymous = anonymous;
        }
        public bool Anonymous { get; set; }
    }
    public class Method {
        public static async Task<object?> Invoke(object instance, string methodName, object parameters) {
            var json = JsonSerializer.Serialize(parameters);
            return await Invoke(instance, methodName, JsonSerializer.Deserialize<JsonElement>(json));
        }
        public static async Task<object?> Invoke(object instance, string methodName, JsonElement jsonParameters) {
            var methodInfos = ExtractMethodInfos(instance.GetType(), methodName, jsonParameters);
            switch (methodInfos.Count()) {
                case 0:
                    throw new Exception("Method was not found or unavailable");
                case 1:
                    var parameters = ExtractParameters(methodInfos.Single(), jsonParameters);
                    if (methodInfos.Single().GetCustomAttribute<AsyncStateMachineAttribute>() != null) {
                        var task = methodInfos.Single().Invoke(instance, parameters.ToArray());
                        if (task != null)
                            return await (dynamic)task;
                        return null;
                    }
                    return methodInfos.Single().Invoke(instance, parameters.ToArray());
                default:
                    throw new Exception("Method signature was ambiguous");
            }

        }
        public static IEnumerable<MethodInfo> ExtractMethodInfos(Type type) {
            var results = type.GetMethods().Where(x => x.GetCustomAttribute<MethodAttribute>() != null);
            results = results.Where(x => x.IsPublic);
            return results;
        }
        public static IEnumerable<MethodInfo> ExtractMethodInfos(System.Type type, string methodName) {
            return ExtractMethodInfos(type).Where(x => x.Name.Equals(methodName, StringComparison.InvariantCultureIgnoreCase));
        }
        public static IEnumerable<MethodInfo> ExtractMethodInfos(Type type, string methodName, object parameters) {
            var json = JsonSerializer.Serialize(parameters);
            return ExtractMethodInfos(type, methodName, JsonSerializer.Deserialize<JsonElement>(json));
        }
        public static IEnumerable<MethodInfo> ExtractMethodInfos(Type type, string methodName, JsonElement jsonParameters) {
            var methodInfos = ExtractMethodInfos(type, methodName);
            foreach (var methodInfo in methodInfos) {
                if (ValidateMethodInfo(methodInfo, jsonParameters))
                    yield return methodInfo;
            }
        }
        public static IEnumerable<object?> ExtractParameters(MethodInfo methodInfo, object parameters) {
            var json = JsonSerializer.Serialize(parameters);
            return ExtractParameters(methodInfo, JsonSerializer.Deserialize<JsonElement>(json));
        }
        public static IEnumerable<object?> ExtractParameters(MethodInfo methodInfo, JsonElement jsonParameters) {
            foreach (var parameterInfo in methodInfo.GetParameters()) {
                switch (jsonParameters.ValueKind) {
                    case JsonValueKind.Object:
                        var jsonProperty = jsonParameters.EnumerateObject().SingleOrDefault(x => x.Name == parameterInfo.Name);
                        yield return ExtractParameter(parameterInfo, jsonProperty);
                        break;
                }
            }
        }
        public static object? ExtractParameter(ParameterInfo parameterInfo, JsonProperty jsonProperty) {
            return JsonSerializer.Deserialize(jsonProperty.Value, parameterInfo.ParameterType);
        }

        public static Validation ValidateMethodInfo(MethodInfo methodInfo, object parameters) {
            var json = JsonSerializer.Serialize(parameters);
            return ValidateMethodInfo(methodInfo, JsonSerializer.Deserialize<JsonElement>(json));
        }
        public static Validation ValidateMethodInfo(MethodInfo methodInfo, JsonElement jsonParameters) {
            Validation results = jsonParameters.EnumerateObject().Count() == methodInfo.GetParameters().Count();
            if (!results)
                return results;
            foreach (var parameterInfo in methodInfo.GetParameters()) {
                switch (jsonParameters.ValueKind) {
                    case JsonValueKind.Object:
                        var jsonProperty = jsonParameters.EnumerateObject().SingleOrDefault(x => x.Name == parameterInfo.Name);
                        results[parameterInfo.Name] = ValidateParameterInfo(parameterInfo, jsonProperty);
                        break;
                    default:
                        return "Invalid Token";
                }
            }
            return results;
        }
        public static Validation ValidateParameterInfo(ParameterInfo parameterInfo, JsonProperty jsonProperty) {
            try {
                JsonSerializer.Deserialize(jsonProperty.Value, parameterInfo.ParameterType);
                return true;
            }
            catch(Exception e) {
                return e.Message;
            }
        }

    }
}

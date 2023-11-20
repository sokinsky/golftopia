using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GTA.Web {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodAttribute : Attribute {
        public MethodAttribute() : this(true) { }
        public MethodAttribute(bool anonymous) {
            this.Anonymous = anonymous;
        }
        public bool Anonymous { get; set; }
    }
    public class Method {
        public Method(MethodInfo methodInfo) {
            this.Info = methodInfo;
        }
        [JsonIgnore]
        public MethodInfo Info { get; }
        public string Name => this.Info.Name;
        public Dictionary<string, string> Parameters {
            get {
                var result = new Dictionary<string, string>();
                foreach (var parameterInfo in this.Info.GetParameters()) {
                    if (parameterInfo.Name != null) {
                        result[parameterInfo.Name] = parameterInfo.ParameterType.Name;
                    }
                }
                return result;
            }
        }
        public string Return => this.Info.ReturnType.Name;
        

        public static async Task<object?> Invoke(object instance, string methodName, object parameters) {
            var json = JsonSerializer.Serialize(parameters);
            return await Invoke(instance, methodName, JsonSerializer.Deserialize<JsonElement>(json));
        }
        public static async Task<object?> Invoke(object instance, string methodName, JsonElement jsonParameters) {
            var methods = GetMethodInfos(instance.GetType(), methodName, jsonParameters);
            switch (methods.Count()) {
                case 0:
                    var signature = $"{methodName}({string.Join(",", jsonParameters.EnumerateObject().Select(x => x.Name))})";
                    throw Web.Exception.NotFound($"{signature} is unavailable");
                case 1:
                    var method = methods.Single();
                    var methodInfo = method.Info;
                    var parameters = GetValues(jsonParameters, methods.Single());
                    if (methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>() != null) {
                        var task = methods.Single().Info.Invoke(instance, parameters.ToArray());
                        if (task != null)
                            return await (dynamic)task;
                        return null;
                    }
                    return methodInfo.Invoke(instance, parameters.ToArray());
                default:
                    throw new Exception("Method signature was ambiguous");
            }

        }
        public static Validation CheckValues(JsonElement jsonElement, MethodInfo methodInfo) {
            var result = new Validation();
            foreach (var parameterInfo in methodInfo.GetParameters()) {
                if (parameterInfo.Name != null) {
                    result[parameterInfo.Name] = checkValue(jsonElement, parameterInfo);
                }
            }
            return result;
        }
        private static Validation checkValue(JsonElement jsonParameters, ParameterInfo parameterInfo) {
            try {
                var jsonProperty = getJsonProperty(jsonParameters, parameterInfo);
                if (jsonProperty == null)
                    return false;
                JsonSerializer.Deserialize(jsonProperty.Value.Value, parameterInfo.ParameterType);
            }
            catch (Exception e) {
                return e.Message;
            }
            return true;
        }


        public static IEnumerable<Method> GetMethodInfos(Type type) {
            var results = type.GetMethods().Where(x => x.GetCustomAttribute<MethodAttribute>() != null);
            results = results.Where(x => x.IsPublic);
            return results.Select(x => new Method(x));
        }
        public static IEnumerable<Method> GetMethodInfos(Type type, string methodName) {
            return GetMethodInfos(type).Where(x => x.Name.Equals(methodName, StringComparison.InvariantCultureIgnoreCase));
        }
        public static IEnumerable<Method> GetMethodInfos(Type type, string methodName, object parameters) {
            var json = JsonSerializer.Serialize(parameters);
            return GetMethodInfos(type, methodName, JsonSerializer.Deserialize<JsonElement>(json));
        }
        public static IEnumerable<Method> GetMethodInfos(Type type, string methodName, JsonElement jsonParameters) {
            var methods = GetMethodInfos(type, methodName);
            foreach (var method in methods) {
               if (method.Info.GetParameters().Count() == jsonParameters.EnumerateObject().Count()) {
                    var nameMatch = true;
                    foreach (var parameterInfo in method.Info.GetParameters()) {
                        if (!jsonParameters.EnumerateObject().Any(x => x.Name.Equals(parameterInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
                            nameMatch = false;
                    }
                    if (nameMatch) {
                        if (CheckValues(jsonParameters, method.Info))
                            yield return method;
                    }                    
                }

            }
        }
        public static IEnumerable<object?> GetValues(JsonElement jsonParameters, Method method) {
            return method.Info.GetParameters().Select(x => getValue(jsonParameters, x));
        }
        private static object? getValue(JsonElement jsonParameters, ParameterInfo parameterInfo) {
            var jsonProperty = getJsonProperty(jsonParameters, parameterInfo);
            if (jsonProperty == null)
                throw new System.Exception($"Unable to extract parameter({parameterInfo.Name}) value");
            return JsonSerializer.Deserialize(jsonProperty.Value.Value, parameterInfo.ParameterType);
        }
        private static JsonProperty? getJsonProperty(JsonElement jsonElement, ParameterInfo parameterInfo) {
            if (jsonElement.ValueKind != JsonValueKind.Object)
                throw Exception.BadRequest("Parameters are invalid.  Object expected");            
            var jsonProperties = jsonElement.EnumerateObject().Where(x => x.Name.Equals(parameterInfo.Name, StringComparison.InvariantCultureIgnoreCase));
            switch (jsonProperties.Count()) {
                case 0:
                    return null;
                case 1:
                    return jsonProperties.Single();
                default:
                    throw new System.Exception($"Invalid Web Method Collection.  Ambiguous Parameter({parameterInfo.Name})");
            }
        }
    }
}

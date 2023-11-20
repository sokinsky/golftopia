using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using GTA.Data;
using GTA.Web;

namespace GTA.Server
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute
    {
        public ControllerAttribute(Type type) { Type = type; }
        public Type Type { get; set; }
    }
    public interface IController<out TModel>
        where TModel : Model, new()
    {

        Context Context { get; }
        TModel Model { get; }

        Web.Validation Import(object model);
        Dictionary<string, object> Export();

    }
    public class Controller<TModel> : IController<TModel>
        where TModel : Model, new()
    {
        public Controller(Context context, TModel model) { Context = context; Model = model; }
        public Context Context { get; }
        public TModel Model { get; }

        [Web.Method]
        public object Test() {
            return this.Model.ID;
        }

        public Validation Import(object model) {
            return this.import(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(model)));
        }
        protected virtual Validation import(JsonElement jsonElement) {
            Validation validation = true;
            if (jsonElement.ValueKind != JsonValueKind.Object)
                throw Web.Exception.BadRequest("Invalid Token");
            foreach (var jsonProperty in jsonElement.EnumerateObject()) {
                var property = typeof(TModel).GetProperty(jsonProperty.Name);
                if (property == null)
                    validation[jsonProperty.Name] = $"{jsonProperty.Name} is not a property on ${typeof(TModel).Name}";
                else {
                    try {
                        property.SetValue(this.Model, JsonSerializer.Deserialize(jsonProperty.Value, property.PropertyType));
                    }
                    catch(System.Exception ex) {
                        validation[jsonProperty.Name] = ex.Message;
                    }
                }
            }
            return validation;

            

        }
        protected virtual Validation import(PropertyInfo propertyInfo, JsonProperty jsonProperty) {
            if (typeof(IEnumerable<Model>).IsAssignableFrom(propertyInfo.PropertyType)) {
                return "Unable to import Model[] Property";
            }
            else if (typeof(Model).IsAssignableFrom(propertyInfo.PropertyType)) {
                return "Unable to import Model Property";
            }
            else {
                try {
                    var value = JsonSerializer.Deserialize(jsonProperty.Value, propertyInfo.PropertyType);
                    propertyInfo.SetValue(this.Model, value);
                    return true;
                }
                catch(System.Exception ex) {
                    return ex.Message;
                }
            }
            

        }

        public void Import(TModel model) {
            var properties = typeof(TModel).GetProperties().Where(x => x.SetMethod != null && x.SetMethod.IsPublic);
            properties = properties.Where(x => !typeof(IEnumerable<Model>).IsAssignableFrom(x.PropertyType) && !typeof(Model).IsAssignableFrom(x.PropertyType));


        }
        public Dictionary<string, object> Export() {
            var result = new Dictionary<string, object>();
            var properties = typeof(TModel).GetProperties().Where(x => x.GetMethod != null && x.GetMethod.IsPublic && x.GetCustomAttribute<NotMappedAttribute>() == null);
            properties = properties.Where(x => !typeof(IEnumerable<Model>).IsAssignableFrom(x.PropertyType) && !typeof(Model).IsAssignableFrom(x.PropertyType));
            foreach (var property in properties) {
                var value = property.GetValue(this.Model);
                if (value != null)
                    result[property.Name] = value;
            }
            return result;
        }
    }
    public class ControllerSerializer : JsonConverterFactory {
        public override bool CanConvert(Type typeToConvert) {
            return typeof(IController<Model>).IsAssignableFrom(typeToConvert);
        }
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
            if (typeof(IController<Model>).IsAssignableFrom(typeToConvert)) {
                var converterType = typeof(ControllerConverter<>).MakeGenericType(new Type[] { typeToConvert });
                var converter = Activator.CreateInstance(
                    converterType,
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { options },
                    culture: null);
                if (converter != null)
                    return (JsonConverter)converter;
            }
            return null;
        }

    }
    public class ControllerConverter<TController> : JsonConverter<TController>
        where TController : IController<Model> {
        public ControllerConverter(JsonSerializerOptions options) { }
        public override TController Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }
        public override void Write(Utf8JsonWriter writer, TController controller, JsonSerializerOptions options) {
            JsonSerializer.Serialize(writer, controller.Export(), options);
        }
    }
}

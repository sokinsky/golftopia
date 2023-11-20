using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LMS.Data {
    public class ModelAttribute : Attribute {
        public ModelAttribute(Type type) {
            this.Type = type;
        }
        public Type Type { get; set; }
    }
    [JsonConverter(typeof(ModelConverter))]
    public class Model {
    }

    public class ModelConverter : JsonConverter<Model> {
        public ModelConverter() { }
        public ModelConverter(JsonSerializerOptions options) { }

        public override Model? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Model model, JsonSerializerOptions options) {
            Dictionary<string, object?> results = new Dictionary<string, object?>();
            var properties = model.GetType().GetProperties().Where(x => x.GetMethod != null && x.GetMethod.IsPublic && x.GetCustomAttribute<NotMappedAttribute>() == null);
            foreach (var property in properties) {
                if (!property.PropertyType.IsAssignableFrom(typeof(Model)) && ! property.PropertyType.IsAssignableFrom(typeof(IEnumerable<Model>))){
                    results[property.Name] = property.GetValue(model);
                }
            }
            JsonSerializer.Serialize(writer, results, options);
        }
    }
}

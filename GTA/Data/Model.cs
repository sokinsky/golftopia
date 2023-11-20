using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GTA.Data {
    public class Model {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
    }
    //public class ModelConverter<TModel> : JsonConverter<TModel> where TModel:Model, new() {
    //    public ModelConverter(JsonSerializerOptions options) { }
    //    public override TModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    //        throw new NotImplementedException();
    //    }
    //    public override void Write(Utf8JsonWriter writer, TModel model, JsonSerializerOptions options) {
    //        var data = new Dictionary<string, object>();
    //        foreach (var propertyInfo in this.Properties) {
    //            if (propertyInfo != null) {
    //                var value = propertyInfo.GetValue(model);
    //                if (value != null) {
    //                    data[propertyInfo.Name] = value;
    //                }
    //            }                
    //        }            
    //    }
    //    protected IEnumerable<PropertyInfo> Properties {
    //        get {
    //            var result = typeof(TModel).GetProperties().Where(x => x.SetMethod != null && x.GetMethod != null && x.SetMethod.IsPublic && x.GetMethod.IsPublic && x.GetCustomAttribute<NotMappedAttribute>() == null);
    //            return result.Where(x => ! typeof(IEnumerable<Model>).IsAssignableFrom(x.PropertyType) && ! typeof(Model).IsAssignableFrom(x.PropertyType));
    //        }
    //    }

    //}
}

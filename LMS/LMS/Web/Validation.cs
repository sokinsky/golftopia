using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LMS.Web {
    [JsonConverter(typeof(ValidationConverter))]
    public class Validation : Dictionary<string, Validation>{
        public static implicit operator Validation(bool valid) => new Validation { Message = valid ? null : "Invalid" };
        public static implicit operator Validation(string message) => new Validation { Message = message };
        public static implicit operator bool(Validation validation) => validation.Valid;
        public string? Message { get; set; }

        public new Validation this[string name] {
            get {
                if (!base.ContainsKey(name)) 
                    base[name] = new Validation();
                return base[name];
            }
            set {
                base[name] = value;
            }
        }
        public bool Valid {
            get {
                if (this.Keys.Any())
                    return this.Values.All(validation => validation.Valid);
                return string.IsNullOrWhiteSpace(this.Message);
            }
        }
    }
    public class ValidationConverter : JsonConverter<Validation> {
        public ValidationConverter() { }
        public ValidationConverter(JsonSerializerOptions options) { }
        public override Validation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var innerConverter = new ValidationConverter();

            var validation = new Validation();
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();
            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return validation;
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException();
                string? propertyName = reader.GetString();
                if (propertyName == null)
                    throw new JsonException();
                reader.Read();
                switch (reader.TokenType) {
                    case JsonTokenType.String:
                        var message = reader.GetString();
                        if (message != null)
                            validation[propertyName] = message;
                        break;
                    case JsonTokenType.StartObject:
                        validation[propertyName] = innerConverter.Read(ref reader, typeof(Validation), options);
                        break;
                    default:
                        throw new JsonException();
                }
            }
            throw new JsonException();
        }
        public override void Write(Utf8JsonWriter writer, Validation validation, JsonSerializerOptions options) {
            if (!validation.Keys.Any()) {
                System.Text.Json.JsonSerializer.Serialize(writer, validation.Message, options);
            }
            else {
                var innerResults = new Dictionary<string, Validation>();
                foreach (var key in validation.Keys) {
                    if (! validation[key])
                        innerResults[key] = validation[key];
                }
                System.Text.Json.JsonSerializer.Serialize(writer, innerResults, options);
            }




        }
    }
}

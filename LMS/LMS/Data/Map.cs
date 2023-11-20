using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMS.Data {
    public class Map : Dictionary<string, Map> {
        public Map() { }
        public Map(object init) : this(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(init))) { }
        public Map(JsonElement jsonElement) {

        }
        public object Value { get; set; } = true;
    }
    public class Map<T> : Map {
        public Map() {
            var properties = typeof(T).GetProperties().Where(x => x.GetMethod != null && x.GetMethod.IsPublic && x.GetCustomAttribute<NotMappedAttribute>() == null);

        }
    }
}

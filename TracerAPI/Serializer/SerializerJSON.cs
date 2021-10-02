using System.Text.Json;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI.Serializer
{
    public class SerializerJSON : ISerializer
    {
        private static SerializerJSON instance;
        private JsonSerializerOptions options;

        public object JsonIgnoreCondition { get; private set; }

        private SerializerJSON() {
            options = new JsonSerializerOptions() {
                IgnoreNullValues = true,
                IncludeFields = true
            };
            
        }

        public static SerializerJSON GetInstance() {
            if (instance == null) 
            { instance = new SerializerJSON(); }
            return instance;
        }

        public string Serialize(TraceResult obj)
        {
            return JsonSerializer.Serialize(obj, typeof(TraceResult),options);
        }
    }
}

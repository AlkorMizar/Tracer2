using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI.Serializer
{
    class SerializerJSON : ISerializer
    {
        private SerializerJSON instance;
        private JsonSerializerOptions options;

        public object JsonIgnoreCondition { get; private set; }

        private SerializerJSON() {
            options = new JsonSerializerOptions() {
                IgnoreNullValues = true,
                IncludeFields = true
            };
            
        }

        public SerializerJSON GetInstance() {
            if (instance == null) 
            { instance = new SerializerJSON(); }
            return instance;
        }
        public TraceResult Deserialize(string data)
        {
            return (TraceResult)JsonSerializer.Deserialize(data, typeof(TraceResult), options);
        }

        public string Serialize(TraceResult obj)
        {
            return JsonSerializer.Serialize(obj, typeof(TraceResult),options);
        }
    }
}

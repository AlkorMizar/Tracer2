using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI.Serializer
{
    class SerializerJSON : ISerializer
    {
        private SerializerJSON instance;

        private SerializerJSON() { }

        public SerializerJSON GetInstance() {
            if (instance == null) 
            { instance = new SerializerJSON(); }
            return instance;
        }
        public TraceResult Deserialize(string data)
        {
            return (TraceResult)JsonSerializer.Deserialize(data, typeof(TraceResult));
        }

        public string Serialize(TraceResult obj)
        {
            return JsonSerializer.Serialize(obj, typeof(TraceResult));
        }
    }
}

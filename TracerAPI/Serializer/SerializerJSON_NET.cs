using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI.Serializer
{
    public class SerializerJSON_NET:ISerializer
    {
        private static SerializerJSON_NET instance;
        private JsonSerializer serializer;

        public object JsonIgnoreCondition { get; private set; }

        private SerializerJSON_NET()
        {
        }
        public static SerializerJSON_NET GetInstance()
        {
            if (instance == null)
            { instance = new SerializerJSON_NET(); }
            return instance;
        }
        public string Serialize(TraceResult obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}

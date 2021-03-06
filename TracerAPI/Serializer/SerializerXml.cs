using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI.Serializer
{
    class SerializerXml : ISerializer
    {
        private static SerializerXml instance;

        private SerializerXml() { }

        public static SerializerXml GetInstance()
        {
            if (instance == null)
            { instance = new SerializerXml(); }
            return instance;
        }
        public TraceResult Deserialize(string data)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TraceResult));
            using (var sr = new StringReader(data))
            {
                return (TraceResult)xmlSerializer.Deserialize(sr);
            }
        }

        public string Serialize(TraceResult obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TraceResult));
            using (var sw = new StringWriter())
            {
                xmlSerializer.Serialize(sw, obj);
                return sw.ToString();
            }
        }
    }
}

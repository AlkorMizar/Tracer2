using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI.Serializer
{
    class SerializerXml : ISerializer
    {
        private SerializerXml instance;

        private SerializerXml() { }

        public SerializerXml GetInstance()
        {
            if (instance == null)
            { instance = new SerializerXml(); }
            return instance;
        }
        public TraceResult Deserialize(string data)
        {
            DataContractSerializer xmlSerializer = new DataContractSerializer(typeof(TraceResult));
            using (var sr = new StringReader(data))
            {
                using (var xr = XmlReader.Create(sr))
                    return (TraceResult)xmlSerializer.ReadObject(xr);
            }
        }

        public string Serialize(TraceResult obj)
        {
            DataContractSerializer xmlSerializer = new DataContractSerializer(typeof(TraceResult));
            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw))
                {
                    xmlSerializer.WriteObject(xw, obj);
                }
                return sw.ToString();
            }
        }
    }
}

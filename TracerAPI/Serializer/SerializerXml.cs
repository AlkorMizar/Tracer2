using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TraceResult));
            using (TextReader textReader = new StringReader(data))
            {
                return (TraceResult)xmlSerializer.Deserialize(textReader);
            }
        }

        public string Serialize(TraceResult obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TraceResult));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }
    }
}

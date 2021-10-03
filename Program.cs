using System;
using System.Threading;
using Tracer2.TracerAPI;
using Tracer2.TracerAPI.Data;
using Tracer2.TracerAPI.Serializer;
using Tracer2.TracerAPI.Writer;

namespace Tracer2
{
    class Program
    {
        private static ITracer _tracer;
        private static ThreadWatch watch;
        public static void Main()
        {

            _tracer = new Tracer();
            _tracer.StartTrace();
            B clB = new B(_tracer);
            clB.MethodB1();
            clB.MethodB2();
            _tracer.StopTrace();
            IWriter wr = new ConsoleWriter();
            ISerializer ser = SerializerJSON_NET.GetInstance();
            wr.Write(ser.Serialize(_tracer.GetTraceResult()));
        }

    }
}

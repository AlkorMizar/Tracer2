using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer2.TracerAPI;
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
            M0();
            Thread th = new Thread(M0);
            th.Start();
            Thread th2 = new Thread(M1);
            th2.Start();
            th.Join();
            th2.Join();
            IWriter wr = new ConsoleWriter();
            ISerializer ser = SerializerXml.GetInstance();
            wr.Write(ser.Serialize(_tracer.GetTraceResult()));
        }

        public static void M0()
        {
            M1();
            M2();
            M2();
        }

        private static void M1()
        {         
            _tracer.StartTrace();
            M2();
            M2();
            M2();
            int i = int.MaxValue/100;
            while (i-- > 0) ;
            _tracer.StopTrace();
            
        }

        private static void M2()
        {
            _tracer.StartTrace();
            int i = int.MaxValue / 100;
            while (i-- > 0) ;
            _tracer.StopTrace();
        }
    }
}

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
            M0();
            Thread th = new Thread(M0);
            th.Start();
            Thread th2 = new Thread(M1);
            th2.Start();
            th.Join();
            th2.Join();
            _tracer.StopTrace();
            IWriter wr = new ConsoleWriter();
            ISerializer ser = SerializerJSON_NET.GetInstance();
            wr.Write(ser.Serialize(_tracer.GetTraceResult()));
        }

        public static void M0()
        {
            _tracer.StartTrace();
            M1();
            M2();
            M2();
            _tracer.StopTrace();
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

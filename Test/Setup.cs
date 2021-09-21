using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer2.TracerAPI;

namespace Tracer2.Test
{
    class Setup
    {
        private static ITracer _tracer;

        public static void main() {
            _tracer 
        }

        public void M0()
        {
            M1();
            M2();
        }

        private void M1()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            _tracer.StopTrace();
        }

        private void M2()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            _tracer.StopTrace();
        }
    }
}

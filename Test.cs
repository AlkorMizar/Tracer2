using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer2.TracerAPI;

namespace Tracer2
{
    class Sleep {
        public static void sleep() {
            int i = int.MaxValue/40;
            while (i-- > 0);
        }
    }

    class A {
        ITracer tracer;
        public A(ITracer _tracer)
        {
            tracer = _tracer;
        }

        public void MethodA1() {
            tracer.StartTrace();
            Sleep.sleep();
            MethodA2();
            MethodA3();
            tracer.StopTrace();
        }
        public void MethodA2()
        {
            tracer.StartTrace();
            Sleep.sleep();
            MethodA3();
            tracer.StopTrace();
        }
        public void MethodA3()
        {
            tracer.StartTrace();
            Sleep.sleep();
            tracer.StopTrace();
        }
    }

    class B {

        ITracer tracer;
        A clA;
        C clC;
        public B(ITracer _tracer)
        {
            tracer = _tracer;
            clA = new A(tracer);
            clC = new C(tracer);
        }
        public void MethodB1()
        {
            tracer.StartTrace();
            Sleep.sleep();
            Thread thA = new Thread(clA.MethodA1);
            Thread thC = new Thread(clC.MethodС1);
            thA.Start();
            thC.Start();
            MethodB2();
            thA.Join();
            thC.Join();
            tracer.StopTrace();
        }

        public void MethodB2()
        {
            tracer.StartTrace();
            Sleep.sleep();
            tracer.StopTrace();
        }
    }

    class C {
        ITracer tracer;
        public C(ITracer _tracer)
        {
            tracer = _tracer;
        }
        public void MethodС1()
        {
            tracer.StartTrace();
            Sleep.sleep();
            MethodС2();
            MethodС3();
            MethodС3();
            MethodС2();
            MethodС3();
            tracer.StopTrace();
        }
        public void MethodС2()
        {
            tracer.StartTrace();
            Sleep.sleep();
            MethodС3();
            MethodС3();
            tracer.StopTrace();
        }
        public void MethodС3()
        {
            tracer.StartTrace();
            Sleep.sleep();
            tracer.StopTrace();
        }
    }
}

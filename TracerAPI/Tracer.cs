using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI
{
    public class Tracer : ITracer
    {
        private TraceResult result;
        private ThreadWatch watch;

        public Tracer()
        {
            result = new TraceResult();
            watch = ThreadWatch.getInstance();
        }

        public TraceResult GetTraceResult()
        {
            throw new NotImplementedException();
        }

        void ITracer.StartTrace()
        {
            long start = watch.GetThreadTimes();
            StackTrace stackTrace = new StackTrace();
            //разделить два потока для правильного подсчёта времени
            Thread thread = new Thread(() => {
                result.AddNewMethod(new TreeNodeInfo(stackTrace, start));
            });
            thread.Start();
        }

        void ITracer.StopTrace()
        {
            long end = watch.GetThreadTimes();
            StackTrace stackTrace = new StackTrace();
            //разделить два потока для правильного подсчёта времени
            Thread thread = new Thread(() => {
                result.setTimeAndDeactivate(new TreeNodeInfo(stackTrace, end));
            });
            thread.Start();
        }
    }
}

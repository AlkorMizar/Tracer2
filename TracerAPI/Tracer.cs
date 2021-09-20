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
            return result;
        }

        private (String method, String _class, int thread, String[,] path) GenerateInfo(StackTrace stackTrace) {
            StackFrame[] stackFrames = stackTrace.GetFrames();
            String[,] Path = null;
            if (stackFrames.Length > 1)
            {
                Path = new String[stackFrames.Length - 1, 2];
                int size = stackFrames.Length - 1;
                for (int i = 0; i < size; i++)//get path consisting of methodth from last to 1(not 0 !) 
                {
                    Path[i, 0] = stackFrames[size - i].GetMethod().Name;
                    Path[i, 1] = stackFrames[size - i].GetType().Name;
                }
            }
            return (method: stackFrames[0].GetMethod().Name,
                    _class: stackFrames[0].GetType().Name,
                    thread: Thread.CurrentThread.ManagedThreadId,
                    path  : Path);
        }

        void ITracer.StartTrace()
        {
            long start = watch.GetThreadTimes();
            StackTrace stackTrace = new StackTrace();
            //разделить два потока для правильного подсчёта времени
            Thread thread = new Thread(() => {
                var info = GenerateInfo(stackTrace);
                result.AddNewMethod(info.method,info._class,info.thread,info.path,start);
            });
            thread.Start();
        }

        void ITracer.StopTrace()
        {
            long end = watch.GetThreadTimes();
            StackTrace stackTrace = new StackTrace();
            //разделить два потока для правильного подсчёта времени
            Thread thread = new Thread(() => {
                var info = GenerateInfo(stackTrace);
                result.StopMetod(info.method, info._class, info.thread, info.path, end);
            });
            thread.Start();
        }
    }
}

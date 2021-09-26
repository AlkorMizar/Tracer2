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
        private readonly object balanceLock = new object();
        public Tracer()
        {
            result = new TraceResult();
            watch = ThreadWatch.getInstance();
        }

        public TraceResult GetTraceResult()
        {
            return result;
        }

        private (String method, String _class, int thread, String[,] path) GenerateInfo(StackTrace stackTrace,int threadId) {
            StackFrame[] stackFrames = stackTrace.GetFrames();
            
            String[,] Path = null;
            if (stackFrames.Length > 2)
            {
                int size = stackFrames.Length;
                Path = new String[size - 2, 2];
                for (int i = 0; i < size - 2; i++)//get path consisting of methods from last to 2(not 1 !(it start and stop)) 
                {
                    var method = stackFrames[size - 1 - i].GetMethod();
                    Path[i, 0] = method.Name;
                    Path[i, 1] = method.ReflectedType.Name;
                }
            }
            var m = stackFrames[1].GetMethod();
            return (method: m.Name,
                    _class: m.ReflectedType.Name,
                    thread: threadId,
                    path  : Path);
        }

        void ITracer.StartTrace()
        {
            long start = watch.GetThreadTimes();
            
            StackTrace stackTrace = new StackTrace();
            int threadId = Thread.CurrentThread.ManagedThreadId;
            //разделить два потока для правильного подсчёта времени
            Thread thread = new Thread(() => {
                lock (balanceLock)
                {
                    var info = GenerateInfo(stackTrace, threadId);
                    result.AddNewMethod(info.method, info._class, info.thread, info.path, start);
                }
            });
            thread.Start();
        }

        void ITracer.StopTrace()
        {
            long end = watch.GetThreadTimes();
            StackTrace stackTrace = new StackTrace();
            int threadId = Thread.CurrentThread.ManagedThreadId;
            //разделить два потока для правильного подсчёта времени
            Thread thread = new Thread(() => {
                lock (balanceLock)
                {
                    var info = GenerateInfo(stackTrace, threadId);
                    result.StopMetod(info.method, info._class, info.thread, info.path, end);
                }
            });
            thread.Start();
        }
    }
}

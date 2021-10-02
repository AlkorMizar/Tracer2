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

        private bool IsThreadFunction(System.Reflection.MethodBase method)
        {
            return method.Name.Equals("ThreadStart") &&
                   method.ReflectedType.Name.Equals("ThreadHelper");
        }

        private (MethodInfo current, int thread, MethodInfo[] path) GenerateInfo(StackTrace stackTrace,int threadId) {
            StackFrame[] stackFrames = stackTrace.GetFrames();
            int size = stackFrames.Length;
            MethodInfo[] Path = null;

            var m = stackFrames[1].GetMethod();
            MethodInfo meth = new MethodInfo(m.Name, m.ReflectedType.Name, 0);
            
            if (IsThreadFunction(stackFrames[size - 1].GetMethod()))
            {
                size -= 3;
            }

            if (size > 2)
            {
                meth.ID = stackFrames[2].GetILOffset();
                Path = new MethodInfo[size-2];
                for (int i = 2; i < size; i++)//get path consisting of methods from last to 2(not 1 !(it start and stop)) 
                {
                    var method = stackFrames[i].GetMethod();
                    int id = i == size - 1 ? 0 : stackFrames[i + 1].GetILOffset();
                    Path[size - 1 - i] = new MethodInfo(method.Name, method.ReflectedType.Name, id);
                }
            }
            return (current: meth,
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
                    result.AddNewMethod(info.current, info.thread, info.path, start);
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
                    result.StopMetod(info.current, info.thread, info.path, end);
                }
            });
            thread.Start();
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Tracer2.TracerAPI.Data
{
    [Serializable]
    public class TraceResult
    {
        private ConcurrentQueue<ThreadNode> threads;
       
        public ThreadNode[] Threads { get { return threads.ToArray(); } set { } }

        [JsonIgnore]
        private readonly object balanceLock = new object();

        public TraceResult()
        {
            threads = new ConcurrentQueue<ThreadNode>();
        }
        public void AddNewMethod(MethodInfo current, int threadId, MethodInfo[] path, long startTime)
        {
            ThreadNode thread;
            lock (balanceLock)
            {
                thread = GetThread(threadId);
                if (thread == null)
                {

                    thread = new ThreadNode(threadId, threads.Count);
                    threads.Enqueue(thread);
                }
            }
            thread.StartNewMethod(current,path,startTime);
        }

        public void StopMetod(MethodInfo current, int threadId, MethodInfo[] path, long endTime) 
        {
            ThreadNode thread;
            lock (balanceLock)
            {
                thread = GetThread(threadId);
            }
            
            if (thread != null)
            {
                thread.StopMethod(current, path, endTime);
            }
        }

        public ThreadNode GetThread(int threadId) {
            foreach (var thread in threads)
            {
                if (thread.Id == threadId)
                { return thread; }
            }
            return null;
        } 
    }
}

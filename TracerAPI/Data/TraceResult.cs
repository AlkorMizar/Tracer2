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
        public void AddNewMethod(String name,String className,int threadId,String[,] path, long startTime)
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
            thread.StartNewMethod(name,className,path,startTime);
        }

        public void StopMetod(String name, String className, int threadId, String[,] path, long endTime) 
        {
            ThreadNode thread;
            lock (balanceLock)
            {
                thread = GetThread(threadId);
            }
            
            if (thread != null)
            {
                thread.StopMethod(name, className, path, endTime);
            }
        }

        private ThreadNode GetThread(int threadId) {
            foreach (var thread in threads)
            {
                if (thread.Id == threadId)
                { return thread; }
            }
            return null;
        } 
    }
}

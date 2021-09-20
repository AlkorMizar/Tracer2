using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Data
{
    public class TraceResult
    {
        ConcurrentQueue<ThreadNode> threads;

        public void AddNewMethod(String name,String className,int threadId,String[,] path, long startTime)
        {
            ThreadNode thread = GetThread(threadId);
            if (thread == null)
            {
                thread = new ThreadNode(threadId, threads.Count);
                threads.Enqueue(thread);
            }
            thread.StartNewMethod(name,className,path,startTime);
        }

        public void StopMetod(String name, String className, int threadId, String[,] path, long endTime) {
            ThreadNode thread = GetThread(threadId);
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

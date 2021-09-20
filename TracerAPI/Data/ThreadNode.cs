using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Data
{
    class ThreadNode
    {
        public int Id { get; private set; }
        public int Number { get;private set; }
        
        private readonly MethodNode root;

        public ThreadNode(int id, int number) {
            Id = id;
            Number = number;
            root = new MethodNode();
        }

        public void StartNewMethod(String methodName, String className,String[,] path,long startTime) {
            MethodNode previousMethod = GetMethod(path);
            if (previousMethod == null)
            {
                previousMethod = root;    
            }

            previousMethod.AddInnerMethod(new MethodNode(methodName, className, startTime));

        }
        public void StopMethod(String methodName, String className, String[,] path, long endTime) {
            MethodNode method = GetMethod(path);
            if (method != null)
            {
                method = method.GetInnerMethod(methodName, className);
                method.Stop(endTime);
            }
        }
        private MethodNode GetMethod(String[,] path)
        {
            MethodNode method = root;
            int i = 0;
            while (method != null && i<path.Length) {
                method = method.GetInnerMethod(path[i, 0], path[i, 1]);
                i++;
            }
            return method;
        }
    }
}

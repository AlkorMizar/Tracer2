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
        
        private MethodNode root;

        public ThreadNode(int id, int number) {
            Id = id;
            Number = number;
            root = null;
        }

        public void StartNewMethod(String methodName, String className,String[,] path,long startTime) {
            if (root != null)
            {
                MethodNode previousMethod = GetMethod(path);
                if (previousMethod != null)
                {
                    previousMethod.AddInnerMethod(new MethodNode(methodName, className, startTime));
                }
            }
            else 
            {
                root = new MethodNode(methodName, className, startTime);
            }
        }
        public void StopMethod(String methodName, String className, String[,] path, long startTime) {
            MethodNode method = GetMethod(path);
            if (method != null)
            {

                previousMethod.AddInnerMethod(new MethodNode(methodName, className, startTime));
            }
        }
        private MethodNode GetMethod(String[,] path)
        {
            MethodNode method = root;
            int i = 0;
            while (method != null && i<path.Length && method.Name != path[i, 0] 
                   && method.ClassName != path[i, 1] && method.IsActive) {
                method = method.GetInnerMethod(path[i, 0], path[i, 1]);
                i++;
            }
            return method;
        }
    }
}

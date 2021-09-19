using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Data
{
    class ThreadNode
    {
        private String name;
        public int ID { get;private set; }
        public bool IsActive { get; set; }
        
        private MethodNode root;
        
        public void StartNewMethod() { }
        public void StopMethod() { }
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

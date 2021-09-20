using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Data
{
    [Serializable]
    public class MethodNode
    {
        public String Name { get; private set; }
        public String ClassName { get; private set; }
        private long time;
        public long Time { get { return time; } }
        public bool IsActive { get; private set; }

        //[NonSerialized]
        private ConcurrentStack<MethodNode> innerMethods;
        //[NonSerialized]
        private readonly object balanceLock = new object();
        public MethodNode()
        {
            Name = "";
            ClassName = "";
            innerMethods = new ConcurrentStack<MethodNode>();
            IsActive = true;
            time = 0;
        }

        public MethodNode(String _name, String _className, long start)
        {
            Name = _name;
            ClassName = _className;
            innerMethods = new ConcurrentStack<MethodNode>();
            IsActive = true;
            time = start;
        }

        public void AddInnerMethod(MethodNode method)
        {
            innerMethods.Push(method);
        }

        public void Stop(long end) {
            time -= end;
            IsActive = false;
        }

        public MethodNode GetInnerMethod(String name, String className) {
            
            foreach (var method in innerMethods)
            {
                if (method.IsThatMethod(name,className))
                {
                    return method;
                }
            }
            return null;
        
        }

        private bool IsThatMethod(String name, String className) {
            lock (balanceLock)
            {
                return Name == name && ClassName == className && IsActive;
            }
        }
    }
}

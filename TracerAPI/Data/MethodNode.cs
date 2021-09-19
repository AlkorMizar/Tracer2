using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Data
{
    public class MethodNode
    {
        public String Name { get; set; }
        public String ClassName { get; set; }
        private long time;
        public long Time { get { return time; } }
        public bool IsActive { get; set; }

        private ConcurrentQueue<MethodNode> innerMethods;

        public MethodNode()
        {
            Name = "";
            ClassName = "";
            innerMethods = new ConcurrentQueue<MethodNode>();
            IsActive = true;
        }

        public MethodNode(String _name, String _className, long _time, bool _isThread)
        {
            Name = _name;
            ClassName = _className;
            innerMethods = new ConcurrentQueue<MethodNode>();
            IsActive = true;
        }

        public void AddInnerMethod(MethodNode method)
        {
            innerMethods.Enqueue(method);
        }

        public MethodNode GetInnerMethod(String name, String className) {
            foreach (var method in innerMethods) {
                if (method.Name == name && method.ClassName == className) {
                    return method;
                }
            }
            return null;
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Tracer2.TracerAPI.Data
{
    [DataContract()]
    public class MethodNode
    {
        [DataMember(EmitDefaultValue = false)]
        public String Name { get; private set; }
        [DataMember(EmitDefaultValue = false)]
        public String ClassName { get; private set; }
        [DataMember(EmitDefaultValue = false)]
        public long Time { get; private set; }
        
        [JsonIgnore]
        public bool IsActive { get; private set; }

        [DataMember(EmitDefaultValue = false)]
        private ConcurrentStack<MethodNode> innerMethods;
        
        [JsonIgnore]
        private readonly object balanceLock = new object();
        public MethodNode()
        {
            Name = "";
            ClassName = "";
            innerMethods = new ConcurrentStack<MethodNode>();
            IsActive = true;
            Time = 0;
        }

        public MethodNode(String _name, String _className, long start)
        {
            Name = _name;
            ClassName = _className;
            innerMethods = new ConcurrentStack<MethodNode>();
            IsActive = true;
            Time = start;
        }

        public void AddInnerMethod(MethodNode method)
        {
            innerMethods.Push(method);
        }

        public void Stop(long end) {
            Time -= end;
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

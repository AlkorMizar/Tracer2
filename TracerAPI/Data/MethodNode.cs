using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Tracer2.TracerAPI.Data
{
    [Serializable]
    public class MethodNode
    {
        [ XmlAttribute]
        public String Name { get;   set; }

        [ XmlAttribute]
        public String ClassName { get;   set; }

        private long time;
        [ XmlAttribute]
        public long Time { get;   set; }
        
        [System.Text.Json.Serialization.JsonIgnore, 
         XmlIgnore,
         Newtonsoft.Json.JsonIgnore]
        public bool IsActive { get; private set; }

        private ConcurrentStack<MethodNode> innerMethods;

        [XmlElement(IsNullable = false)]
        public MethodNode[] InnerMethods { get { return innerMethods.ToArray(); } set { } }

        public bool ShouldSerializeInnerMethods()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (InnerMethods.Length != 0);
        }

        [System.Text.Json.Serialization.JsonIgnore,
         XmlIgnore,
         Newtonsoft.Json.JsonIgnore]
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
            time = start;
        }

        public void AddInnerMethod(MethodNode method)
        {
            innerMethods.Push(method);
        }

        public void Stop(long end) {
            Time = (end-time) / 10000;
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

        public IEnumerator<MethodNode> GetEnumerator()
        {
            return innerMethods.GetEnumerator();
        }
    }
}

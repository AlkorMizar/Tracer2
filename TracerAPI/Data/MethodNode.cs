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
        public long Time { 
            get {
                if (IsUnregistered) {
                    time = 0;
                    foreach (var meth in innerMethods)
                    {
                        time += meth.Time;
                    }
                }
                return time;
            } 
            set {  } }
        
        [System.Text.Json.Serialization.JsonIgnore, 
         XmlIgnore,
         Newtonsoft.Json.JsonIgnore]
        public bool IsActive { get; private set; }

        [System.Text.Json.Serialization.JsonIgnore,
         XmlIgnore,
         Newtonsoft.Json.JsonIgnore]
        public bool IsUnregistered { get; private set; }

        private int id;

        private ConcurrentQueue<MethodNode> innerMethods;

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
            id = default(int);
            innerMethods = new ConcurrentQueue<MethodNode>();
            IsActive = true;
            IsUnregistered = true;
            time=0;
        }

        public MethodNode(MethodInfo info, long start)
        {
            Name = info.NameMeth;
            ClassName = info.NameClass;
            id = info.ID;
            innerMethods = new ConcurrentQueue<MethodNode>();
            IsActive = true;
            IsUnregistered = false;
            time = start;
        }

        public MethodNode(MethodInfo info)
        {
            Name = info.NameMeth;
            ClassName = info.NameClass;
            id = info.ID;
            innerMethods = new ConcurrentQueue<MethodNode>();
            IsActive = true;
            IsUnregistered = true;
        }

        public void AddInnerMethod(MethodNode method)
        {
            innerMethods.Enqueue(method);
        }

        public void Stop(long end) {
            time = (end-time) / 10000;
            IsActive = false;
        }

        public MethodNode GetInnerMethod(MethodInfo methodInfo) {
            
            foreach (var method in innerMethods)
            {
                if (method.IsThatMethod(methodInfo))
                {
                    return method;
                }
            }
            return null;
        
        }

        private bool IsThatMethod(MethodInfo info) {
            lock (balanceLock)
            {
                return Name == info.NameMeth && ClassName == info.NameClass
                       && id==info.ID && IsActive;
            }
        }
    }
}

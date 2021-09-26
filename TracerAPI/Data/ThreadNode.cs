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
    public class ThreadNode
    {
        [XmlAttribute]
        public int Id { get;   set; }

        [XmlAttribute]
        public long Time { get;   set; }

        [System.Text.Json.Serialization.JsonIgnore,
         XmlIgnore,
         Newtonsoft.Json.JsonIgnore]
        public int Number { get;  set; }

        private readonly MethodNode root;
        public MethodNode[] Root { get { return root.InnerMethods; } set { } }

        [System.Text.Json.Serialization.JsonIgnore, 
         XmlIgnore,
         Newtonsoft.Json.JsonIgnore]
        private readonly object balanceLock = new object();

        public ThreadNode() { }

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
            if (method == null)
            {
                method = root;
            }
            method = method.GetInnerMethod(methodName, className);
            if (method != null)
            {
                method.Stop(endTime);
                RecountTime();
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
        private void RecountTime()
        {
            Time = 0;
            foreach(var rootMethod in root)
            {
                if (!rootMethod.IsActive)
                {
                    Time += rootMethod.Time;
                }
            }
        }
    }
}

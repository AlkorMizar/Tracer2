using System;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Tracer2.TracerAPI.Data
{
    [Serializable]
    public class ThreadNode
    {
        [XmlIgnore,
         JsonIgnore]
        public int Id { get;   set; }

        [XmlAttribute(AttributeName = "time"),
         JsonProperty("time")]
        public long Time { get { return root.Time; }   set { } }

        [XmlAttribute(AttributeName ="id"),
         JsonProperty("id")]
        public int Number { get;  set; }

        private readonly MethodNode root;
        public MethodNode[] methods { get { return root.methods; } set { } }

        [XmlIgnore,
         JsonIgnore]
        private readonly object balanceLock = new object();

        public ThreadNode() { }

        public ThreadNode(int id, int number) {
            Id = id;
            Number = number+1;
            root = new MethodNode();
        }


        private MethodNode CreatePath(MethodInfo[] path) {
            MethodNode method = root,
                       previous=method;
            int i = 0;
            while (method != null && i < path.Length)
            {
                previous = method;
                method = method.GetInnerMethod(path[i]);
                i++;
            }
            for (i--; i <path.Length; i++)
            {
                MethodNode newMeth = new MethodNode(path[i]);
                previous.AddInnerMethod(newMeth);
                previous = newMeth;
            }
            return previous;
        }

        public void StartNewMethod(MethodInfo methodInfo,MethodInfo[] path,long startTime) {
            MethodNode previousMethod = GetMethod(path);
            if (previousMethod == null)
            {
                previousMethod = CreatePath(path);    
            }
            previousMethod.AddInnerMethod(new MethodNode(methodInfo, startTime));
        }
        public void StopMethod(MethodInfo methodInfo, MethodInfo[] path, long endTime) {
            MethodNode method = GetMethod(path);
            if (method == null)
            {
                method = root;
            }
            method = method.GetInnerMethod(methodInfo);
            if (method != null)
            {
                method.Stop(endTime);
            }
            
        }
        public MethodNode GetMethod(MethodInfo[] path)
        {
            MethodNode method = root;
            if (path != null)
            {
                int i = 0;
                while (method != null && i < path.Length)
                {
                    method = method.GetInnerMethod(path[i]);
                    i++;
                }
            }
            return method;
        }
    }
}

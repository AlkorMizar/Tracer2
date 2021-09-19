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

        ConcurrentBag<MethodNode> methodsTree;
    }
}

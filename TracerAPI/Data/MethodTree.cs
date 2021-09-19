using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Data
{
    public class MethodTree
    {
        MethodNode root;

        public MethodTree() { }
        public void StartNewMethod() { }
        public void StopMethod() { }
        private MethodNode GetMethod() {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer2.TracerAPI.Data
{
    public class MethodInfo
    {
        public string NameMeth { get; set; }
        public string NameClass { get; set; }
        public int ID { get; set; }

        public MethodInfo(string method,string classN,int id)
        {
            NameMeth = method;
            NameClass = classN;
            ID = id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI.Serializer
{
    interface ISerializer
    {
        String Serialize(TraceResult obj);
    }
}

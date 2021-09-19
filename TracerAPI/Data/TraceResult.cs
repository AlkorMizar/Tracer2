using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Data
{
    public class TraceResult
    {
        ConcurrentBag<ThreadNode> threads;

    }
}

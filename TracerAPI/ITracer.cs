using System;
using System.Collections.Generic;
using System.Text;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI
{
    public interface ITracer
    {
        // вызывается в начале замеряемого метода
        void StartTrace();

        // вызывается в конце замеряемого метода
        void StopTrace();

        // получить результаты измерений
        TraceResult GetTraceResult();
    }
}

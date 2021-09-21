using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Writer
{
    interface IWriter
    {
        void WriteAsync(String data);
    }
}

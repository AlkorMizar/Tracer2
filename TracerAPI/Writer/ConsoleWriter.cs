using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Writer
{
    class ConsoleWriter : IWriter
    {
        public void WriteAsync(string data)
        {
            Console.WriteLine(data);
        }
    }
}

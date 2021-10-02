using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer2.TracerAPI.Writer
{
    public class ConsoleWriter : IWriter
    {
        public void Write(string data)
        {
            Console.WriteLine(data);
        }
    }
}

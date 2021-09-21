using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tracer2.TracerAPI.Writer
{
    class FileWriter : IWriter
    {
        public void WriteAsync(string data)
        {

            try
            {
                using (StreamWriter sw = new StreamWriter("result.txt", false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tracer2.TracerAPI.Writer
{
    public class FileWriter : IWriter
    {
        public void Write(string data)
        {

            try
            {
                using (StreamWriter sw = new StreamWriter("..\\..\\..\\result.txt", false, System.Text.Encoding.Default))
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

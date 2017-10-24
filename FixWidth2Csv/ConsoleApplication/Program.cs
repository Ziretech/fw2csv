using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixWidth2Csv;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = Encoding.GetEncoding("ISO-8859-1");
            var converter = new FixWidthParser {Writer = new Writer()};
            converter.ConvertText(new Reader());
        }
    }

    class Writer : IWriter
    {
        public void WriteRow(string line)
        {
            Console.Write(line + "#*#");
        }
    }

    class Reader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}

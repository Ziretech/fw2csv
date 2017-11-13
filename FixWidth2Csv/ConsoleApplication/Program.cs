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
        private const string rowDelimiter = "#*#";
        private const string cellDelimiter = "{";

        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = Encoding.GetEncoding("ISO-8859-1");
            var converter = new FixWidthParser {Writer = new Writer {RowDelimiter = rowDelimiter}, CellDelimiter = cellDelimiter};
            converter.ConvertText(new Reader { Delimiters = new[] {rowDelimiter, cellDelimiter}});
        }
    }

    class Writer : IWriter
    {
        public string RowDelimiter { get; set; }

        public void WriteRow(string line)
        {
            Console.Write(line + RowDelimiter);
        }
    }

    class Reader : IReader
    {
        public string ReadLine()
        {
            var line = Console.ReadLine();
            foreach (var delimiter in Delimiters)
            {
                if (!string.IsNullOrEmpty(line) && line.Contains(delimiter))
                {
                    throw new ArgumentException($"Source text contain delimiter sequence ({delimiter})");
                }
            }
            return line;
        }

        public string ReadCharacters(int numberOfCharacters)
        {
            throw new NotImplementedException();
        }

        public string[] Delimiters { get; set; }
    }
}

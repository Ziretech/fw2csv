using System;
using System.Collections.Generic;
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
}

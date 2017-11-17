using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixWidth2Csv;

namespace ConsoleApplication
{
    class Program
    {
        private const string RowDelimiter = "#*#";
        private const string CellDelimiter = "{";

        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                var inputFilePath = args[0];
                var outputFilePath = args[1];

                using (var outputStream = File.OpenWrite(outputFilePath))
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        var writer = new Writer(outputStream, new CsvConverter(CellDelimiter), RowDelimiter);
                        var reader = new Reader(inputStream);
                        var converter = new ConvertFixWidthToMatrix {Writer = writer};
                        converter.Convert(reader);
                    }
                }
            }
            else
            {
                Console.Out.WriteLine("usage: <input file> <output file>");
            }
        }
    }
}

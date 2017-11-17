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
        private const string DefaultRowDelimiter = "#*#";
        private const string DefaultCellDelimiter = "{";

        static void Main(string[] args)
        {
            if (args.Length >= 2 && args.Length < 6)
            {
                var inputFilePath = args[0];
                var outputFilePath = args[1];

                var bufferSize = 10000;
                var cellDelimiter = DefaultCellDelimiter;
                var rowDelimiter = DefaultRowDelimiter;

                if (args.Length >= 3)
                {
                    bufferSize = int.Parse(args[2]);
                }

                if (args.Length >= 4)
                {
                    cellDelimiter = args[3];
                }

                if (args.Length == 5)
                {
                    rowDelimiter = args[4];
                }

                Console.Out.WriteLine("");
                
                using (var outputStream = File.OpenWrite(outputFilePath))
                using (var inputStream = File.OpenRead(inputFilePath))
                {
                    var writer = new Writer(outputStream, new CsvConverter(cellDelimiter), rowDelimiter);
                    var reader = new Reader(inputStream, bufferSize, new []{cellDelimiter, rowDelimiter});
                    var converter = new ConvertFixWidthToMatrix { Writer = writer };
                    converter.Convert(reader);
                }
            }
            else
            {
                Console.Out.WriteLine("usage: <input file> <output file> [<buffer size> [[<cell delimiter> [<row delimiter>]]]");
            }
        }
    }
}

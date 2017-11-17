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
        private const string RowDelimiter = "#*#";
        private const string CellDelimiter = "{";

        static void Main(string[] args)
        {
            using (var outputStream = File.OpenWrite(@"c:\temp\fil.txt"))
            {
                using (var inputStream = File.OpenRead(@"c:\temp\fixwidthexempel.txt"))
                {
                    var writer = new Writer(outputStream, new CsvConverter(CellDelimiter), RowDelimiter);
                    var reader = new Reader(inputStream);
                    var converter = new ConvertFixWidthToMatrix { Writer = writer };
                    converter.Convert(reader);
                }
            }
        }
    }
}

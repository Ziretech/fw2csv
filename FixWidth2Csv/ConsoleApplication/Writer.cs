using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixWidth2Csv;

namespace ConsoleApplication
{
    public class Writer : IWriter
    {
        public string RowDelimiter { get; set; }

        public void WriteRow(string line)
        {
            Console.Write(line + RowDelimiter);
        }
    }
}

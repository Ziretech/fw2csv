using FixWidth2Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2CsvTest
{
    class WriterMock : IWriter
    {
        public List<string> WriteList { get; private set; }

        public WriterMock()
        {
            WriteList = new List<string>();
        }

        public void WriteRow(string line)
        {
            WriteList.Add(line);
        }
    }
}

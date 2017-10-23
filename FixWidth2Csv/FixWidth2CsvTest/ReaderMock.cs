using FixWidth2Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2CsvTest
{
    class ReaderMock : IReader
    {
        private List<string> lines;
        private int linesRead;

        public ReaderMock()
        {
            lines = new List<string>();
        }

        public string ReadLine()
        {
            if (lines.Count <= linesRead)
            {
                return null;
            }
                
            return lines[linesRead++];
        }

        internal void AddLine(string line)
        {
            lines.Add(line);
        }
    }
}

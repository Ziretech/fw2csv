using System;
using System.Collections.Generic;
using FixWidth2Csv;

namespace FixWidth2CsvTest
{
    public class ReaderMock : IReader
    {
        private readonly Queue<string> _lines = new Queue<string>();

        public void AddLine(string line)
        {
            _lines.Enqueue(line);
        }

        public string ReadLine(int minimalNumberOfCharacters)
        {
            var line = _lines.Dequeue();

            if (line.Length < minimalNumberOfCharacters)
            {
                throw new ArgumentException($"Requested minimal linesize ({minimalNumberOfCharacters}) is shorter than expected string \"{line}\"");
            }
            
            return line;
        }
    }
}
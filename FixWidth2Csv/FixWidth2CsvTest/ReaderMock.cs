using System;
using System.Collections.Generic;
using FixWidth2Csv;

namespace FixWidth2CsvTest
{
    public class ReaderMock : IReader
    {
        private readonly Queue<Line> _lines = new Queue<Line>();

        public bool MoreLines => _lines.Count > 0;

        public void AddLine(string line, int expectedMinNumOfCharacters)
        {
            _lines.Enqueue(new Line { Text = line, ExpectedMinNumOfCharacters = expectedMinNumOfCharacters});
        }

        public string ReadLine(int minimalNumberOfCharacters)
        {
            var line = _lines.Dequeue();

            if (line.Text.Length < minimalNumberOfCharacters)
            {
                throw new ArgumentException($"Requested minimal linesize ({minimalNumberOfCharacters}) is shorter than expected string \"{line.Text}\"");
            }

            if (line.ExpectedMinNumOfCharacters != minimalNumberOfCharacters)
            {
                throw new ArgumentException($"Minimum number of characters ({minimalNumberOfCharacters}) not same as expected ({line.ExpectedMinNumOfCharacters}).");
            }
            
            return line.Text;
        }

        private class Line
        {
            public string Text { get; set; }
            public int ExpectedMinNumOfCharacters { get; set; }
        }
    }
}
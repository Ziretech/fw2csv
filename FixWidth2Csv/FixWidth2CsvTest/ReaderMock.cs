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
        private readonly List<string> _lines;
        private int _linesRead;
        private int _charactersRead;

        public ReaderMock()
        {
            _lines = new List<string>();
            _linesRead = 0;
            _charactersRead = 0;
        }

        public string ReadLine()
        {
            if (_lines.Count <= _linesRead)
            {
                return null;
            }
                
            return _lines[_linesRead++].Substring(_charactersRead);
        }

        public string ReadCharacters(int numberOfCharacters)
        {
            if (_lines.Count == _linesRead)
            {
                return null;
            }

            var line = _lines[_linesRead];

            var characters = line.Substring(_charactersRead, numberOfCharacters);

            _charactersRead += numberOfCharacters;

            if (_charactersRead == line.Length)
            {
                _charactersRead = 0;
                _linesRead++;
            }

            return characters;
        }

        internal void AddLine(string line)
        {
            _lines.Add(line);
        }
    }
}

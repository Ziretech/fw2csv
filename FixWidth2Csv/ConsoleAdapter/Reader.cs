using FixWidth2Csv;
using System;
using System.IO;
using System.Text;

namespace ConsoleAdapter
{
    public class Reader : IReader
    {
        private readonly Stream _stream;
        private readonly Encoding _encoding;
        private readonly ByteBuffer _buffer;
        private readonly int _bufferSize;
        private readonly string[] _disallowedStrings;
        private bool _firstRead;
        
        public Reader(Stream stream, int bufferSize = 1024, string[] disallowedStrings = null)
        {
            _stream = stream;
            _encoding = Encoding.UTF8;
            _bufferSize = bufferSize;
            _buffer = new ByteBuffer(_bufferSize);
            _disallowedStrings = disallowedStrings;
            _firstRead = true;
        }

        public bool MoreLines
        {
            get
            {
                _buffer.FillBuffer(_stream);
                return !_buffer.IsEmpty;
            }
        }

        public string ReadLine(int minimalNumberOfCharacters)
        {
            if (_firstRead)
            {
                RemoveByteOrderMark();
                _firstRead = false;
            }

            _buffer.FillBuffer(_stream);
            var lineLength = _buffer.NextLineLength(minimalNumberOfCharacters);
            if (lineLength == _bufferSize)
            {
                throw new InvalidOperationException($"Line length exceeds buffersize {_bufferSize}");
            }
            var line = _buffer.GetString(_encoding, lineLength);
            if (_disallowedStrings != null)
            {
                foreach (var disallowedString in _disallowedStrings)
                {
                    if (line.Contains(disallowedString))
                    {
                        throw new InvalidOperationException($"Line contains disallowed characters \"{disallowedString}\"");
                    }
                }
            }
            _buffer.MoveBytesLeft(lineLength);
            _buffer.RemoveEndLine();
            return line;
        }

        private void RemoveByteOrderMark()
        {
            _buffer.FillBuffer(_stream);
            if (_buffer.BeginWithSequence(0xEF, 0xBB, 0xBF))
            {
                _buffer.MoveBytesLeft(3);
            }
        }
    }
}

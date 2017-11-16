using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixWidth2Csv;

namespace ConsoleApplication
{
    class Reader : IReader
    {
        private readonly Stream _stream;
        private readonly Encoding _encoding;
        private readonly ByteBuffer _buffer;
        private readonly int _bufferSize;
        
        public Reader(Stream stream, int bufferSize = 1024)
        {
            _stream = stream;
            _encoding = Encoding.UTF8;
            _bufferSize = bufferSize;
            _buffer = new ByteBuffer(_bufferSize);
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
            _buffer.FillBuffer(_stream);
            var lineLength = _buffer.NextLineLength(minimalNumberOfCharacters);
            if (lineLength == _bufferSize)
            {
                throw new InvalidOperationException($"Line length exceeds buffersize {_bufferSize}");
            }
            var line = _buffer.GetString(_encoding, lineLength);
            _buffer.MoveBytesLeft(lineLength);
            _buffer.RemoveEndLine();
            return line;
        }
    }
}

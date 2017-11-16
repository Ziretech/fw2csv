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
        private const byte CarriageReturn = 13; // '\r'
        private const byte Endline = 10;        // '\n'
        private readonly Stream _stream;
        private readonly Encoding _encoding;
        private readonly byte[] _readBuffer;
        private int _readPosition;
        private bool _atEndOfFile;

        public Reader(Stream stream, int bufferSize = 1024)
        {
            _stream = stream;
            _encoding = Encoding.UTF8;
            _readBuffer = new byte[bufferSize];
            _readPosition = 0;
            _atEndOfFile = false;
        }

        public bool MoreLines { get; }

        private void FillFromStream(int numberOfCharacters)
        {
            var readBytes = _stream.Read(_readBuffer, _readPosition, numberOfCharacters);
            _readPosition += readBytes;
        }

        //public string ReadLine(int minimalNumberOfCharacters)
        //{
        //    var lineBreakFoundAt = -1;
        //    do
        //    {
        //        for (var i = minimalNumberOfCharacters; i < _readPosition || lineBreakFoundAt != -1; i++)
        //        {
        //            if (_readBuffer[i] == '\n')
        //            {
        //                lineBreakFoundAt = i;
        //            }
        //        }
        //    }
        //    while()
            

        //    var streamLine = _encoding.GetString(buffer, 0, charsRead);

        //    return streamLine;
        //}

        public string ReadLine(int minimalNumberOfCharacters)
        {
            try
            {
                var readBytes = _stream.Read(_readBuffer, _readPosition, 1024);
                _readPosition += readBytes;

                int limiterSize = 1;

                var index = Array.IndexOf(_readBuffer, Endline, 0, _readPosition);
                if (index < 0)
                {
                    index = readBytes;
                }
                else if (index > 0 && _readBuffer[index - 1] == CarriageReturn)
                {
                    index--;
                    limiterSize = 2;
                }

                return _encoding.GetString(_readBuffer, 0, index);
            }
            catch (ArgumentException exception)
            {
                throw new InvalidOperationException($"An input file line exceeded size of buffer ({_readBuffer.Length})", exception);
            }
            
        }
    }
}

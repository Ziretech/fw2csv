using System;
using System.IO;

namespace ConsoleApplication
{
    class BufferReader : IBufferReader, IDisposable
    {
        private readonly FileStream _stream;
        private readonly int _bufferSize;
        private bool _endOfFile;
        private int _readCharacters;

        public BufferReader(string filepath, int bufferSize)
        {
            _stream = File.Open(filepath, FileMode.Open);
            _bufferSize = bufferSize;
            _endOfFile = false;
        }

        public byte[] Read()
        {
            var buffer = new byte[_bufferSize];

            _readCharacters = _stream.Read(buffer, 0, buffer.Length);

            if (_readCharacters < _bufferSize)
            {
                var tempBuffer = buffer;
                buffer = new byte[_readCharacters];
                for (var i = 0; i < _readCharacters; i++)
                {
                    buffer[i] = tempBuffer[i];
                }
            }

            return buffer;
            //content.Append(Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public bool EndOfFile => _readCharacters < 1;
    }
}
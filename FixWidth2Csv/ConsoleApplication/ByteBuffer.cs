using System;
using System.IO;
using System.Text;

namespace ConsoleApplication
{
    class ByteBuffer
    {
        private const byte CarriageReturn = 13; // '\r'
        private const byte Endline = 10;        // '\n'

        private readonly byte[] _buffer;
        private int _numberOfCharactersRead;

        public ByteBuffer(int bufferSize)
        {
            _buffer = new byte[bufferSize];
            _numberOfCharactersRead = 0;
        }

        public void FillBuffer(Stream stream)
        {
            _numberOfCharactersRead = stream.Read(_buffer, 0, _buffer.Length);
        }

        internal string GetString(Encoding encoding, int numberOfCharacters)
        {
            if (numberOfCharacters > _numberOfCharactersRead)
            {
                throw new ArgumentException($"Requested number of characters to read ({numberOfCharacters}) exceeding number of characters available({_numberOfCharactersRead}).");
            }
            var result = encoding.GetString(_buffer, 0, numberOfCharacters);

            _numberOfCharactersRead -= numberOfCharacters;

            return result;
        }

        public void MoveBytesLeft(int moveLength)
        {
            Array.Copy(_buffer, moveLength, _buffer, 0, _numberOfCharactersRead - moveLength);
        }

        // fill buffer
        // is buffer empty
        // get part of buffer
        public int NextLineLength(int minNumberOfBytes)
        {
            return Math.Min(Math.Min(IndexOfEnd(minNumberOfBytes, 0), IndexOfEnd(minNumberOfBytes, Endline)), IndexOfEnd(minNumberOfBytes, CarriageReturn));
        }

        private int IndexOfEnd(int startIndex, byte endCondition)
        {
            var index = Array.IndexOf(_buffer, endCondition, startIndex, _numberOfCharactersRead - startIndex);
            return index < startIndex ? _numberOfCharactersRead : index;
        }
    }
}
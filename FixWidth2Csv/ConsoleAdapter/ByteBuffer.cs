using System;
using System.IO;
using System.Text;

namespace ConsoleAdapter
{
    class ByteBuffer
    {
        private const byte CarriageReturn = 13; // '\r'
        private const byte Endline = 10;        // '\n'

        private readonly byte[] _buffer;
        private int _numberOfCharactersRead;

        public bool IsEmpty => _numberOfCharactersRead == 0;

        public ByteBuffer(int bufferSize)
        {
            _buffer = new byte[bufferSize];
            _numberOfCharactersRead = 0;
        }

        public void FillBuffer(Stream stream)
        {
            _numberOfCharactersRead += stream.Read(_buffer, _numberOfCharactersRead, _buffer.Length - _numberOfCharactersRead);
        }

        internal string GetString(Encoding encoding, int numberOfCharacters)
        {
            if (numberOfCharacters > _numberOfCharactersRead)
            {
                throw new ArgumentException($"Requested number of characters to read ({numberOfCharacters}) exceeding number of characters available({_numberOfCharactersRead}).");
            }
            var result = encoding.GetString(_buffer, 0, numberOfCharacters);

            return result;
        }

        public void MoveBytesLeft(int moveLength)
        {
            Array.Copy(_buffer, moveLength, _buffer, 0, _numberOfCharactersRead - moveLength);
            _numberOfCharactersRead -= moveLength;
        }

        public int NextLineLength(int minNumberOfBytes)
        {
            if (_numberOfCharactersRead < minNumberOfBytes)
            {
                throw new InvalidOperationException($"Line is shorter ({_numberOfCharactersRead}) than required minimum number of bytes ({minNumberOfBytes}). ");
            }

            return Math.Min(Math.Min(IndexOfEnd(minNumberOfBytes, 0), IndexOfEnd(minNumberOfBytes, Endline)), IndexOfEnd(minNumberOfBytes, CarriageReturn));
        }

        private int IndexOfEnd(int startIndex, byte endCondition)
        {
            var index = Array.IndexOf(_buffer, endCondition, startIndex, _numberOfCharactersRead - startIndex);
            return index < startIndex ? _numberOfCharactersRead : index;
        }

        public void RemoveEndLine()
        {
            var numToRemove = 0;

            if (BeginWithSequence(Endline))
            {
                numToRemove = 1;
            }
            else if (BeginWithSequence(CarriageReturn, Endline))
            {
                numToRemove = 2;
            }

            if (numToRemove > 0)
            {
                MoveBytesLeft(numToRemove);
            }
        }

        public bool BeginWithSequence(params byte[] sequence)
        {
            if (sequence.Length > _numberOfCharactersRead)
            {
                return false;
            }

            for (var i = 0; i < sequence.Length; i++)
            {
                if (_buffer[i] != sequence[i])
                    return false;
            }
            return true;
        }
    }
}
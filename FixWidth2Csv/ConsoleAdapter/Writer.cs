using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixWidth2Csv;

namespace ConsoleAdapter
{
    public class Writer : IWriter
    {
        private readonly Stream _stream;
        private readonly string _lineSeparator;
        private readonly Encoding _encoding;
        private readonly IConverter _converter;

        public Writer(Stream outputStream, IConverter converter, string lineSeparator)
        {
            _stream = outputStream;
            _lineSeparator = lineSeparator;
            _encoding = Encoding.GetEncoding("ISO-8859-1");
            _converter = converter;
        }

        public void WriteRow(string[] columns)
        {
            var row = _converter.ConvertRow(columns) + _lineSeparator;
            var info = _encoding.GetBytes(row);
            _stream.Write(info, 0, info.Length);
        }
    }
}

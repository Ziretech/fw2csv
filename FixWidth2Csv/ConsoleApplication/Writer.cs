using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixWidth2Csv;

namespace ConsoleApplication
{
    public class Writer : IWriter
    {
        private readonly Stream _stream;
        private readonly string _separator;
        private Encoding _encoding;
        private IConverter _converter;

        public Writer(Stream outputStream, IConverter converter)
        {
            _stream = outputStream;
            _separator = ";";
            _encoding = Encoding.GetEncoding("ISO-8859-1");
            _converter = converter;
        }

        public void WriteRow(string[] columns)
        {
            var row = "";
            foreach (var column in columns)
            {
                row += (row == "" ? "" : _separator) + column;
            }

            row = _converter.ConvertRow(columns) + Environment.NewLine;
            var info = _encoding.GetBytes(row);
            _stream.Write(info, 0, info.Length);
        }
    }
}

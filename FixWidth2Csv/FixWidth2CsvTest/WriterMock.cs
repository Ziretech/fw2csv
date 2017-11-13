using System;
using System.Collections.Generic;
using FixWidth2Csv;
using NUnit.Framework;

namespace FixWidth2CsvTest
{
    public class WriterMock : IWriter
    {
        public WriterMock()
        {
            RowList = new List<string[]>();

        }

        public void WriteRow(string[] columns)
        {
            RowList.Add(columns);
        }

        public List<string[]> RowList { get; }
    }
}
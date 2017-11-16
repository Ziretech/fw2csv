using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class CsvConverter : IConverter
    {
        private readonly string _cellSeparator;

        public CsvConverter(string cellSeparator = ";")
        {
            _cellSeparator = cellSeparator;
        }

        public string ConvertRow(string[] cells)
        {
            var row = cells[0];

            foreach (var cell in cells.Skip(1))
            {
                row += _cellSeparator + cell;
            }
            
            return row;
        }
    }
}

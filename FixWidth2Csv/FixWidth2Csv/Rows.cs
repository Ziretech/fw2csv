using System;
using System.Collections.Generic;

namespace FixWidth2Csv
{
    internal class Rows
    {
        private string _rowString;
        private IEnumerable<int> _cellWidths;

        public Rows(string rowString, IEnumerable<int> cellWidths)
        {
            _rowString = rowString;
            _cellWidths = cellWidths;
        }

        public IEnumerable<string> GetCells()
        {
            var index = 0;
            foreach(var width in _cellWidths)
            {
                yield return GetCell(index, width);
                index += width + 1;
            }
        }

        internal string GetCell(int start, int length)
        {
            var adjustedLength = start + length <= _rowString.Length ? length : length - ((start + length) - _rowString.Length);

            return _rowString.Substring(start, adjustedLength).TrimEnd(' ');
        }
    }
}
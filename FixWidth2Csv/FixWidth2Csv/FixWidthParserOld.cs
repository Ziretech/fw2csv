using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2Csv
{
    public class FixWidthParserOld
    {
        public FixWidthParserOld()
        {
            CellDelimiter = ";";
        }

        public IWriterOld Writer { private get; set; }
        public string CellDelimiter { get; set; }

        public void ConvertText(IReaderOld reader)
        {
            var currentLine = 1;
            try
            {
                var headerLine = reader.ReadLine();
                var widths = GetColumnWidths(reader.ReadLine()).ToArray();
                Writer.WriteRow(ConvertRow(headerLine, widths));
                var data = reader.ReadLine();
                while (data != null)
                {
                    data = GetRow(reader, data, widths);
                    Writer.WriteRow(ConvertRow(data, widths));
                    currentLine ++;
                    data = reader.ReadLine();
                }
            }
            catch (ArgumentException exception)
            {   
                throw new ArgumentException(exception.Message + $" on line {currentLine}", exception);
            }
        }

        private string GetRow(IReaderOld reader, string data, int[] widths)
        {
            var nextPartOfRow = "";
            while (nextPartOfRow != null && IsBrokenRow(data, widths))
            {
                nextPartOfRow = reader.ReadLine();
                if (nextPartOfRow != null)
                {
                    data += Environment.NewLine + nextPartOfRow;
                }
                
            }
            return data;
        }

        internal bool IsBrokenRow(string row, int[] widths)
        {
            int firstWidthsLength = widths.Sum() - widths[widths.Length - 1] + widths.Length - 1;

            return row.Length < firstWidthsLength;
        }

        public string ConvertRow(string rowLine, IEnumerable<int> widths)
        {
            var csvline = "";

            foreach(var width in widths)
            {
                if (string.IsNullOrEmpty(rowLine))
                {
                    throw new ArgumentException("Row did not contain enough cells");
                }
                if(string.IsNullOrEmpty(csvline))
                {
                    csvline = GetCell(rowLine, width);
                }
                else
                {
                    csvline += CellDelimiter + GetCell(rowLine, width);
                }
                rowLine = GetRemainingCells(rowLine, width);
            }
            if (!string.IsNullOrEmpty(rowLine))
            {
                throw new ArgumentException("Row contain too many cells");
            }
            return csvline;
        }

        public IEnumerable<int> GetColumnWidths(string widthline)
        {
            if (IsNotDefinedText(widthline))
            {
                throw new ArgumentException("No delimiters was found.");
            }

            var widths = new List<int>();

            foreach (var delimiter in widthline.Split(' '))
            {
                if (delimiter.Length < 1)
                {
                    throw new ArgumentException("Space between delimiter can't be more than one space.");
                }
                widths.Add(delimiter.Length);
            }

            return widths;
        }

        internal string GetRemainingCells(string rowLine, int width)
        {
            if (rowLine.Length <= width)
            {
                return "";
            }
                
            if (!IsCellFollowedBySpace(rowLine, width))
            {
                throw new ArgumentException("Cells are not separated by space");
            }

            return rowLine.Substring(width + 1);
        }

        private bool IsCellFollowedBySpace(string rowLine, int width)
        {
            return rowLine.Substring(width, 1).Equals(" ");
        }

        internal string GetCell(string cells, int width)
        {
            return cells.Length <= width ? cells.TrimEnd() : cells.Substring(0, width).TrimEnd();
        }

        private bool IsNotDefinedText(string headerLine)
        {
            return headerLine == null || headerLine.Trim().Length < 1;
        }
    }
}

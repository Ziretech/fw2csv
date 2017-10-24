using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2Csv
{
    public class FixWidthParser
    {
        public FixWidthParser()
        {
            CellDelimiter = ";";
        }

        public IWriter Writer { private get; set; }
        public string CellDelimiter { get; set; }

        public string ConvertHeader(string headerLine)
        {
            if (IsNotDefinedText(headerLine))
            {
                throw new ArgumentException("No header was found.");
            }

            var csvline = "";
            foreach (var header in headerLine.Split(' '))
            {
                if (!string.IsNullOrEmpty(header))
                {
                    if (string.IsNullOrEmpty(csvline))
                    {
                        csvline += header;
                    }
                    else
                    {
                        csvline += CellDelimiter + header;
                    }
                }
            }

            return csvline;
        }

        public void ConvertText(IReader reader)
        {
            Writer.WriteRow(ConvertHeader(reader.ReadLine()));
            var widths = GetColumnWidths(reader.ReadLine()).ToArray();
            var data = reader.ReadLine();
            while(data != null)
            {
                data = GetRow(reader, data, widths);

                Writer.WriteRow(ConvertRow(data, widths));
                data = reader.ReadLine();
            }
            
        }

        private string GetRow(IReader reader, string data, int[] widths)
        {
            var nextPartOfRow = "";
            while (nextPartOfRow != null && IsBrokenRow(data, widths))
            {
                nextPartOfRow = reader.ReadLine();
                data += Environment.NewLine + nextPartOfRow;
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
            return rowLine.Length > width ? rowLine.Substring(width+1) : "";
        }

        internal string GetCell(string cells, int width)
        {
            return cells.Length < width ? cells.Trim() : cells.Substring(0, width).Trim();
        }

        private bool IsNotDefinedText(string headerLine)
        {
            return headerLine == null || headerLine.Trim().Length < 1;
        }
    }
}

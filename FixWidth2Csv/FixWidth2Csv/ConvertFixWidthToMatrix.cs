using System;
using System.Linq;
using Entity;

namespace FixWidth2Csv
{
    public class ConvertFixWidthToMatrix
    {
        public IWriter Writer { get; set; }

        public void Convert(IReader reader)
        {
            var headers = reader.ReadLine(1);
            var delimiters = new Delimiters(reader.ReadLine(1));
            var widths = delimiters.GetColumnWidths();
            var minRowLength = delimiters.GetMinimumRequiredRowWidth();

            Writer.WriteRow(new Rows(headers, widths).GetCells().ToArray());

            var currentLineNumber = 3;
            try
            {
                while (reader.MoreLines)
                {
                    var rows = reader.ReadLine(minRowLength);
                    Writer.WriteRow(new Rows(rows, widths).GetCells().ToArray());
                    currentLineNumber ++;
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Conversion error occured during processing of line {currentLineNumber}", exception);
            }
            
        }
    }
}

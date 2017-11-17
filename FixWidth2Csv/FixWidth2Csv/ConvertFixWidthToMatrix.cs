using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            while(reader.MoreLines)
            {
                var rows = reader.ReadLine(minRowLength);
                Writer.WriteRow(new Rows(rows, widths).GetCells().ToArray());
            }
        }
    }
}

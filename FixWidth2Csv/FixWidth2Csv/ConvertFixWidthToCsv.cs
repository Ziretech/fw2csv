using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2Csv
{
    public class ConvertFixWidthToCsv
    {
        public IWriter Writer { get; set; }

        public void Convert(IReader reader)
        {
            var headers = reader.ReadLine(1);
            var widths = new Delimiters(reader.ReadLine(1)).GetColumnWidths();
            

            Writer.WriteRow(new Rows(headers, widths).GetCells().ToArray());

            while(reader.MoreLines)
            {
                var rows = reader.ReadLine(1);
                Writer.WriteRow(new Rows(rows, widths).GetCells().ToArray());
            }
            
        }
    }
}

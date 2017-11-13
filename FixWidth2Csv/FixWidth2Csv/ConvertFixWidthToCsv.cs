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
            var delimiters = new Delimiters(reader.ReadLine(1));
            var rows = reader.ReadLine(1);

            var headerRow = new Rows(headers, delimiters.GetColumnWidths());

            Writer.WriteRow(new[] { headers });
            Writer.WriteRow(new[] { rows });
        }
    }
}

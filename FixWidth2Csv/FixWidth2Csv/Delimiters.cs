using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2Csv
{
    public class Delimiters
    {
        private string _delimiters;

        public Delimiters(string delimiters)
        {
            _delimiters = delimiters;
        }

        public IEnumerable<int> GetColumnWidths()
        {
            foreach(var delimiter in _delimiters.Split(' '))
            {
                yield return delimiter.Length;
            }
        }

        public int GetMinimumRequiredRowWidth()
        {
            var minimumRequiredRowWidth = 0;

            foreach (var width in GetColumnWidths().Reverse().Skip(1))
            {
                minimumRequiredRowWidth += width + 1;
            }
            
            return minimumRequiredRowWidth;
        }
    }
}

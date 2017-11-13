using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixWidth2Csv;

namespace ConsoleApplication
{
    public class Reader : IReader
    {
        public string ReadLine()
        {
            var line = Console.ReadLine();
            foreach (var delimiter in Delimiters)
            {
                if (!string.IsNullOrEmpty(line) && line.Contains(delimiter))
                {
                    throw new ArgumentException($"Source text contain delimiter sequence ({delimiter})");
                }
            }
            return line;
        }

        public string ReadCharacters(int numberOfCharacters)
        {
            throw new NotImplementedException();
        }

        public string[] Delimiters { get; set; }
    }
}

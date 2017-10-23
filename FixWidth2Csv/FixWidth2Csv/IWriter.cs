using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2Csv
{
    interface IWriter
    {
        void Write(string line);
    }
}

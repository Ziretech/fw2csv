﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixWidth2Csv
{
    public interface IWriter
    {
        void WriteRow(string[] columns);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class EmptyStringsAreLast : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (String.IsNullOrEmpty(y) && !String.IsNullOrEmpty(x))
            {
                return -1;
            }
            else if (!String.IsNullOrEmpty(y) && String.IsNullOrEmpty(x))
            {
                return 1;
            }
            else
            {
                return String.Compare(x, y);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace BD2012.Code
{
    public class Burndown
    {
        public Burndown()
        {
            Rows = new List<LineItem>();
            Values = new List<Values>();
        }
        public List<LineItem> Rows;    // order matters
        public List<Values> Values;

        public string ProjectName { get; set;  }
    }
}
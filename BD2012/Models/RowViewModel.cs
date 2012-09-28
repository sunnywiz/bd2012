

using System.Collections.Generic;
using System.Linq;
using bd2012.data;
using System;

namespace BD2012.Models
{
    public class RowViewModel
    {
        public RowViewModel()
        {
            Children = new List<RowViewModel>();
        }

        public string Hierarchy { get; set; } 
        public int LineItemId { get; set;  }
        public string Name { get; set;  }
        public int PixelIndent { get; set;  }

        public decimal? Low { get; set; }
        public decimal? High { get; set; }
        public decimal? Left { get; set; }
        public decimal? Actual { get; set; }

        public bool IsCalculated { get; set; }

        public List<RowViewModel> Children { get; set; }
        public RowViewModel Parent { get; set; }

        public RowViewModel CopyFrom(LineItem item)
        {
            throw new NotImplementedException("Left off here - need to construct a view based on data in a project "); 
        }


    }
}
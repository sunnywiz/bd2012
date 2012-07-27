

using System.Collections.Generic;
using System.Linq;
using BD2012.Code;

namespace BD2012.Models
{
    public class BurndownViewModel
    {
        public List<RowViewModel> Rows { get; set; }

        public BurndownViewModel CopyFrom(Burndown bd)
        {
            this.Rows = bd.Rows.Select(x => new RowViewModel().CopyFrom(x)).ToList();
            return this; 
        }
    }

    public class RowViewModel
    {
        public int LineItemId { get; set;  }
        public string Name { get; set;  }
        public int PixelIndent { get; set;  }

        public RowViewModel CopyFrom(LineItem item)
        {
            LineItemId = item.Id; 
            Name = item.Name;
            PixelIndent = LineItemId*10;
            return this;
        }
    }


}
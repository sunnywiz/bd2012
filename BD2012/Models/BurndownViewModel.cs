

using System.Collections.Generic;

namespace BD2012.Models
{
    public class BurndownViewModel
    {

        public List<BurndownRowModel> Rows { get; set; }
    }

    public class BurndownRowModel
    {
        public int LineItemId { get; set;  }
        public string Name { get; set;  }
        public int PixelIndent { get; set;  }
    }

}
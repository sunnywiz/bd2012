using System.Collections.Generic;

namespace BD2012.Code
{
    public class LineItem
    {
        public int Id { get; set; }   // tracks when moved around
        public string Name { get; set; }
        public LineItem Parent { get; set; }  // this is the one that matters

        public List<LineItem> Children { get; set;  }  // calculated
        public int? Indent { get; set; }  // calculated
    }
}


using System.Collections.Generic;
using System.Linq;
using BD2012.Code;

namespace BD2012.Models
{
    public class BurndownViewModel
    {
        public List<RowViewModel> Rows { get; set; }

        public List<RowViewModel> RootRows { get; set; }

/*        public BurndownViewModel CopyFrom(BurndownDB bd)
        {
            Rows = new List<RowViewModel>(); 
            RootRows = new List<RowViewModel>(); 
            var unassigned = new List<RowViewModel>();
            var dictOrig = new Dictionary<int, LineItem>();
            var dictRvm = new Dictionary<int, RowViewModel>(); 

            foreach (var row in bd.LineItems)
            {
                var rvm = new RowViewModel().CopyFrom(row);
                
                Rows.Add(rvm);
                if (row.Parent == null) { 
                    RootRows.Add(rvm);
                    rvm.Hierarchy = rvm.LineItemId.ToString();
                    rvm.PixelIndent = 0; 
                } else { 
                    unassigned.Add(rvm); 
                }
                dictRvm[row.LineItemId] = rvm;
                dictOrig[row.LineItemId] = row; 
            }

            while (unassigned.Count > 0)
            {
                for (int i = 0; i < unassigned.Count; i++)
                {
                    var iggy = unassigned[i];
                    var orig = dictOrig[iggy.LineItemId];
                    var origParent = orig.Parent; 
                    var iggyparent = dictRvm[origParent.LineItemId];

                    if (iggyparent.Hierarchy != null)
                    {
                        iggyparent.Children.Add(iggy);
                        iggy.Parent = iggyparent;
                        iggy.Hierarchy = iggyparent.Hierarchy + "/" + iggy.LineItemId;
                        iggy.PixelIndent = iggyparent.PixelIndent + 20; 
                        unassigned.RemoveAt(i);
                        i--;
                    }
                }
            }

            foreach (var v in bd.DataPoints.Where(v=>v.When == null))
            {
                var vm = dictRvm[v.LineItemId];
                vm.CopyFrom(v.Snapshot); 
            }

            Rows = Rows.OrderBy(x => x.Hierarchy).ToList();  // top down order
            for (int i = Rows.Count - 1; i >= 0; i--)
            {
                var parent = Rows[i].Parent;

                if (parent != null)
                {
                    var child = Rows[i];
                    if (parent.IsCalculated || 
                        (!parent.High.HasValue && 
                        !parent.Low.HasValue 
                        && !parent.Left.HasValue 
                        && !parent.Actual.HasValue))
                    {
                        parent.IsCalculated = true;
                        if (child.High.HasValue)
                        {
                            var high = parent.High ?? 0m;
                            high += child.High.Value;
                            parent.High = high;
                        }
                    }
                }
            }

            return this; 
        }*/
    }
        
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
            LineItemId = item.LineItemId; 
            Name = item.Name;
            return this;
        }

    }


}
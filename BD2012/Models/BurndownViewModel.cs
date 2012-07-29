

using System.Collections.Generic;
using System.Linq;
using BD2012.Code;

namespace BD2012.Models
{
    public class BurndownViewModel
    {
        public List<RowViewModel> Rows { get; set; }

        public List<RowViewModel> RootRows { get; set; }


        public BurndownViewModel CopyFrom(Burndown bd)
        {
            Rows = new List<RowViewModel>(); 
            RootRows = new List<RowViewModel>(); 
            var unassigned = new List<RowViewModel>();
            var dictOrig = new Dictionary<int, LineItem>();
            var dictRvm = new Dictionary<int, RowViewModel>(); 

            foreach (var row in bd.Rows)
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
                dictRvm[row.Id] = rvm;
                dictOrig[row.Id] = row; 
            }

            while (unassigned.Count > 0)
            {
                for (int i = 0; i < unassigned.Count; i++)
                {
                    var iggy = unassigned[i];
                    var orig = dictOrig[iggy.LineItemId];
                    var origParent = orig.Parent; 
                    var iggyparent = dictRvm[origParent.Id];

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
            return this; 
        }
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

        public List<RowViewModel> Children { get; set; }
        public RowViewModel Parent { get; set; }

        public RowViewModel CopyFrom(LineItem item)
        {
            LineItemId = item.Id; 
            Name = item.Name;
            return this;
        }

        public RowViewModel CopyFrom(Snapshot snapshot)
        {
            Low = snapshot.Low;
            High = snapshot.High;
            return this;  
        }
    }


}
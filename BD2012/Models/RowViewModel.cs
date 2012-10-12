using bd2012.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD2012.Models
{
    public class RowViewModel
    {
        public RowViewModel()
        {
            Children = new List<RowViewModel>();
            Cells = new Dictionary<string, CellViewModel>(); 
        }
        public string Name { get; set; }
        public int Id { get; set; }
        public int Indent { get; set; }
        public List<RowViewModel> Children { get; private set; }
        public RowViewModel Parent { get; set; }

        public Dictionary<string, CellViewModel> Cells { get; set; }

        public RowViewModel CopyFrom(LineItem item)
        {
            Name = item.Name;
            Id = item.LineItemId; 

            return this;
        }

        public static void LinkParentAndChild(RowViewModel parent, RowViewModel child) {
            parent.Children.Add(child);
            child.Parent = parent; 
        }
    }

    public class CellViewModel
    {
        public decimal Value { get; set; }
        public bool IsEditable { get; set; } 
    }
}

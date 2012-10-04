using bd2012.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD2012.Models
{
    public class RowItem
    {
        public RowItem()
        {
            Children = new List<RowItem>();
            Data = new Dictionary<string, decimal>(); 
        }
        public string Name { get; set; }
        public int Id { get; set; }
        public int Indent { get; set; }
        public List<RowItem> Children { get; private set; }
        public RowItem Parent { get; set; }
        public Dictionary<string, decimal> Data { get; set; }

        // for now, leaves are editable and above that are not.  That might change later
        // with locked thingies and stuff. 
        public bool IsEditable { get; set; } 

        public RowItem CopyFrom(LineItem item)
        {
            Name = item.Name;
            Id = item.LineItemId; 

            return this;
        }

        public static void LinkParentAndChild(RowItem parent, RowItem child) {
            parent.Children.Add(child);
            child.Parent = parent; 
        }
    }
}

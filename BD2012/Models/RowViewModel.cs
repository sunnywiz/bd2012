

using System.Collections.Generic;
using System.Linq;
using bd2012.data;
using System;

namespace BD2012.Models
{
    public class BurndownViewModel
    {
        public BurndownViewModel()
        {
            AllRows = new List<RowItem>();
            HierarchicalRows = new List<RowItem>();
            Columns = new List<string>(); 
        }

        public string Name { get; set; }
        public List<RowItem> HierarchicalRows { get; private set; } 
        public List<RowItem> AllRows { get; private set; }
        public List<string> Columns { get; private set; } 

        public BurndownViewModel CopyFrom(Project project)
        {
            Name = project.ProjectName;

            var parentItems = new List<LineItem>();
            var childItems = new List<LineItem>();
            foreach (var item in project.LineItems)
            {
                if (item.Parent == null)
                {
                    parentItems.Add(item);
                }
                else
                {
                    childItems.Add(item);
                }
            }

            Dictionary<int, RowItem> dict = new Dictionary<int, RowItem>();
            HierarchicalRows = new List<RowItem>(); 
            foreach (var item in parentItems)
            {
                var rowItem = new RowItem().CopyFrom(item);
                HierarchicalRows.Add(rowItem);
                dict.Add(rowItem.Id, rowItem);
                rowItem.Indent = 0; 
            }

            var lastCount = childItems.Count + 1;
            while (childItems.Count > 0 && childItems.Count < lastCount)
            {
                for (int i = 0; i < childItems.Count; i++)
                {
                    var item = childItems[i];
                    // find parent 
                    if (dict.ContainsKey(item.Parent.LineItemId))
                    {
                        var parentRow = dict[item.Parent.LineItemId];
                        var newchild = new RowItem().CopyFrom(item);
                        dict.Add(item.LineItemId, newchild); 
                        RowItem.LinkParentAndChild(parentRow, newchild);
                        newchild.Indent = parentRow.Indent + 1;
                        childItems.RemoveAt(i);
                        i--; 
                    }
                }
                lastCount = childItems.Count;
            }

            AllRows = new List<RowItem>();
            RecurseAppend(HierarchicalRows, AllRows);

            foreach (var dataCol in project.ColumnDefinitions)
            {
                Columns.Add(dataCol.ColumnName);
            }

            foreach (var d in project.Data.Where(x => x.Value.HasValue))
            {
                RowItem li; 
                if (dict.TryGetValue(d.LineItem.LineItemId, out li))
                {
                    if (li == null) continue;
                    li.Data[d.Column.ColumnName] = d.Value.Value;
                }
            }

            return this; 
        }

        private void RecurseAppend(List<RowItem> HierarchicalRows, List<RowItem> AllRows)
        {
            foreach (var item in HierarchicalRows)
            {
                AllRows.Add(item);
                if (item.Children.Count > 0)
                {
                    RecurseAppend(item.Children, AllRows);
                }
            }
        }
    }
}
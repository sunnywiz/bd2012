

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
            AllRows = new List<RowViewModel>();
            HierarchicalRows = new List<RowViewModel>();
            ColumnNames = new List<string>();
            ColumnEditability = new List<bool>(); 
        }

        public string Name { get; set; }
        public int ProjectId { get; set; }
        public List<RowViewModel> HierarchicalRows { get; private set; } 
        public List<RowViewModel> AllRows { get; private set; }
        public List<string> ColumnNames { get; private set; }
        public List<bool> ColumnEditability { get; private set; } 

        public BurndownViewModel CopyFrom(Project project)
        {
            ProjectId = project.ProjectId; 
            Name = project.ProjectName;

            List<LineItem> parentItems;
            List<LineItem> childItems;
            SplitIntoParentVsChild(project, out parentItems, out childItems);

            Dictionary<int, RowViewModel> dict = new Dictionary<int, RowViewModel>();
            ConvertParents(parentItems, dict);
            ConvertChildren(childItems, dict);

            AllRows = new List<RowViewModel>();
            RecurseAppend(HierarchicalRows, AllRows);

            MapData(project, dict);

            UpdateEditability(AllRows); 

            foreach (var row in HierarchicalRows) 
                UpdateCalculations(row);

            return this; 
        }

        private void UpdateEditability(List<RowViewModel> AllRows)
        {
            for (int i = 0; i < ColumnNames.Count; i++)
            {
                string colName = ColumnNames[i];
                bool editable = ColumnEditability[i];

                foreach (var row in AllRows)
                {
                    CellViewModel cvm = null;
                    if (row.Cells.TryGetValue(colName, out cvm))
                    {
                        cvm.IsEditable = editable; 
                    }
                }
            }
        }

        private void UpdateCalculations(RowViewModel parentRow)
        {
            if (parentRow.Children != null && parentRow.Children.Count > 0)
            {
                foreach (var row in parentRow.Children) UpdateCalculations(row);

                // i have children, therefore i am calculated
                foreach (var c in ColumnNames)
                {
                    decimal sum = 0m;
                    foreach (var childRow in parentRow.Children)
                    {
                        CellViewModel cvm;
                        if (childRow.Cells.TryGetValue(c, out cvm))
                        {
                            sum += cvm.Value;
                        }
                    }
                    if (!parentRow.Cells.ContainsKey(c))
                        parentRow.Cells[c] = new CellViewModel();
                    parentRow.Cells[c].Value = sum;
                    parentRow.Cells[c].IsEditable = false;
                }
            }
        }

        private void MapData(Project project, Dictionary<int, RowViewModel> dict)
        {
            ColumnNames.Clear();
            ColumnEditability.Clear(); 
            foreach (var dataCol in project.ColumnDefinitions)
            {
                ColumnNames.Add(dataCol.ColumnName);
                ColumnEditability.Add(dataCol.IsImmediateEntry);
            }

            foreach (var d in project.Data.Where(x => x.Value.HasValue))
            {
                RowViewModel li;
                if (dict.TryGetValue(d.LineItem.LineItemId, out li))
                {
                    if (li == null) continue;
                    if (!li.Cells.ContainsKey(d.Column.ColumnName)) li.Cells[d.Column.ColumnName] = new CellViewModel(); 
                    li.Cells[d.Column.ColumnName].Value = d.Value.Value;
                }
            }
        }

        private static void ConvertChildren(List<LineItem> childItems, Dictionary<int, RowViewModel> dict)
        {
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
                        var newchild = new RowViewModel().CopyFrom(item);
                        dict.Add(item.LineItemId, newchild);
                        RowViewModel.LinkParentAndChild(parentRow, newchild);
                        newchild.Indent = parentRow.Indent + 1;
                        childItems.RemoveAt(i);
                        i--;
                    }
                }
                lastCount = childItems.Count;
            }
        }

        private void ConvertParents(List<LineItem> parentItems, Dictionary<int, RowViewModel> dict)
        {
            HierarchicalRows = new List<RowViewModel>();
            foreach (var item in parentItems)
            {
                var rowItem = new RowViewModel().CopyFrom(item);
                HierarchicalRows.Add(rowItem);
                dict.Add(rowItem.Id, rowItem);
                rowItem.Indent = 0;
            }
        }

        private static void SplitIntoParentVsChild(Project project, out List<LineItem> parentItems, out List<LineItem> childItems)
        {
            parentItems = new List<LineItem>();
            childItems = new List<LineItem>();
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
        }

        private void RecurseAppend(List<RowViewModel> HierarchicalRows, List<RowViewModel> AllRows)
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
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace bd2012.data
{
    public class BurndownContext : DbContext 
    {
        public BurndownContext() : base("BurndownCEDatabase")
        {
            
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ColumnDefinition> ColumnDefinitions { get; set; }
    }

    public class LineItem
    {
        public int LineItemId { get; set; }
        public string Name { get; set; }
        public virtual LineItem Parent { get; set; }
        public virtual Project Project { get; set; }
    }

    public class Data
    {
        public int DataId { get; set; }
        public DateTime? When { get; set; }
        public virtual LineItem LineItem { get; set; }
        public virtual ColumnDefinition Column { get; set; }
        public decimal? Value { get; set; } 
    }

    public class Project
    {
        public int ProjectId { get; set; } 
        public string ProjectName { get; set; }
        public virtual ICollection<ColumnDefinition> ColumnDefinitions { get; set; } 
        public virtual ICollection<LineItem> LineItems { get; set; }
        public virtual ICollection<Data> Data { get; set; } 
    }

    public class ColumnDefinition
    {
        public int ColumnDefinitionId { get; set; }
        public string ColumnName { get; set; }
        public int Order { get; set; } 
        public bool IsImmediateEntry { get; set; }
        public virtual Project Project { get; set; }
    }
}
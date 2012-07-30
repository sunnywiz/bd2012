using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using bd2012.data;

namespace BD2012.seed
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseAlways<BurndownContext>());
                using (var context = new BurndownContext())
                    //The trailing 'true' marks that the database is to be dropped and recreated.
                {
                    context.Database.Initialize(true);

                    var project = new Project()
                                      {
                                          ProjectName = "Test Project",
                                          ColumnDefinitions = new List<ColumnDefinition>(),
                                          LineItems = new List<LineItem>(),
                                          Data = new List<Data>()
                                      };
                    context.Projects.Add(project);

                    ColumnDefinition est;
                    ColumnDefinition left;
                    project.ColumnDefinitions.Add(est = new ColumnDefinition()
                                                            {
                                                                ColumnName = "Estimate",
                                                                IsImmediateEntry = false,
                                                                Order = 1
                                                            });

                    project.ColumnDefinitions.Add(left = new ColumnDefinition()
                                                             {
                                                                 ColumnName = "Left",
                                                                 IsImmediateEntry = true,
                                                                 Order = 2
                                                             });

                    LineItem[] l = new LineItem[10];
                    Dictionary<int, bool> isParent = new Dictionary<int, bool>();
                    for (int i = 0; i < l.Length; i++)
                    {
                        isParent[i] = false;
                        l[i] = new LineItem() {Name = "Line " + (i+1)};
                        if (i > 3 && i%2 == 0)
                        {
                            l[i].Parent = l[i - 1];
                            isParent[i - 1] = true;
                        }
                        project.LineItems.Add(l[i]);
                    }

                    int number1 = 2;
                    int number2 = 1;
                    foreach (var child in isParent.Where(x => !x.Value))
                    {
                        var data1 = new Data() {Column = est, LineItem = l[child.Key], Value = number1};
                        var data2 = new Data() {Column = left, LineItem = l[child.Key], Value = number2};
                        number1++;
                        number2++;
                        project.Data.Add(data1);
                        project.Data.Add(data2);
                    }

                    context.SaveChanges();
                }
            }finally
            {
                Console.WriteLine("Done");
            }
        }
    }
}

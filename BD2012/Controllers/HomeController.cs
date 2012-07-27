using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD2012.Code;
using BD2012.Models;

namespace BD2012.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public static int id = 0;

        public ActionResult Burndown()
        {
            var data = new Burndown()
                            {
                                ProjectName = "ADQ Phase II"
                            };
            var projectManagement = new LineItem() {Id = id++, Name = "Project Management"};
            data.Rows.Add(projectManagement);
            var commandLineProcessor = new LineItem() {Id = id++, Name = "Command Line Processor"};
            data.Rows.Add(commandLineProcessor);
            var getFieldListRight = new LineItem()
                                        {
                                            Id = id++,
                                            Name = "time allocated for getting field list correct",
                                            Parent = commandLineProcessor
                                        };

            data.Rows.Add(getFieldListRight);
            var saveRecord = new LineItem()
                                 {
                                     Id = id++,
                                     Name = "Save Record -- Adding SubmissionRecord to DB",
                                     Parent = commandLineProcessor
                                 };

            data.Rows.Add(saveRecord);
            var modifyDbSchema = new LineItem() {Id = id++, Name = "modify db schema", Parent = saveRecord};
            data.Rows.Add(modifyDbSchema);
            var writeOutRecord = new LineItem() {Id = id++, Name = "write out record", Parent = saveRecord};
            data.Rows.Add(writeOutRecord);

            DateTime? d = new DateTime(6/27/2012);
            V(data, projectManagement, 8, 8, 8, d);
            V(data, getFieldListRight, 3, 5, 5, d);
            V(data, modifyDbSchema, 3, 5, 5, d);
            V(data, writeOutRecord, 2, 3, 3, d);

            d = new DateTime(7/2/2012);
            V(data, projectManagement, 8, 8, 6, d);
            V(data, getFieldListRight, 3, 5, 2, d);
            V(data, modifyDbSchema, 3, 5, 2, d);
            V(data, writeOutRecord, 2, 3, 1, d);

            d = null;
            V(data, projectManagement, 8, 8, 4, d);
            V(data, getFieldListRight, 3, 5, 2, d);
            V(data, modifyDbSchema, 3, 5, 0, d);
            V(data, writeOutRecord, 2, 3, 0, d);

            return View(new BurndownViewModel().CopyFrom(data));
        }

        private static void V(Burndown model, LineItem getFieldListRight, decimal low, decimal high, decimal estleft, DateTime? dateTime)
        {
            DateTime? d = null; 
            model.Values.Add(new Values()
                                 {
                                     When = dateTime,
                                     LineItemId = getFieldListRight.Id,
                                     Snapshot =
                                         new Snapshot()
                                             {
                                                 Id = id++,
                                                 Low = low,
                                                 High = high,
                                                 EstLeft = estleft,
                                                 ActualSpent = null
                                             }
                                 });
        }
    }
}

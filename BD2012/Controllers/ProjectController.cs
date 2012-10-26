using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bd2012.data;
using BD2012.Models;

namespace BD2012.Controllers
{ 
    public class ProjectController : Controller
    {
        private BurndownContext db = new BurndownContext();

        //
        // GET: /Project/

        public ViewResult Index()
        {
            return View(db.Projects.ToList());
        }

        //
        // GET: /Project/Details/5

        public ViewResult Details(int id)
        {
            Project project = db.Projects.Find(id);
            return View(project);
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Project/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(project);
        }
        
        //
        // GET: /Project/Edit/5
 
        public ActionResult Edit(int id)
        {
            Project project = db.Projects.Find(id);
            return View(project);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        //
        // GET: /Project/Delete/5
 
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Find(id);
            return View(project);
        }

        //
        // POST: /Project/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // 
        // GET: /Project/Burndown/5
        public ActionResult Burndown(int id)
        {
            Project project = db.Projects.Find(id);
            var vm = new BurndownViewModel().CopyFrom(project);
            return View("Burndown", vm); 
        }

        [HttpPost]
        public ActionResult Burndown(BurndownViewModel model)
        {
            if (model.ProjectId == 0) throw new Exception("missing ProjectId");
            Project project = db.Projects.Find(model.ProjectId); 
            if (project==null) throw new Exception("Yo, project not found");

            var keys = Request.Params.AllKeys;
            for (int i = 0; i < keys.Length; i++)
            {
                if (!keys[i].StartsWith("update-")) continue;

                string[] split = keys[i].Split('-');
                if (split.Length != 3) continue;

                int id = 0;
                if (!int.TryParse(split[1], out id)) continue;

                string colName = split[2];
                if (String.IsNullOrEmpty(colName)) continue;

                decimal newval = 0; 
                if (!decimal.TryParse(Request.Params[keys[i]], out newval)) continue; 

                var row = (from d in project.Data
                           where d.Column.ColumnName == colName
                                 && d.LineItem.LineItemId == id
                           select d).FirstOrDefault(); 
                if (row != null)
                {
                    row.Value = newval; 
                }
            }
            db.SaveChanges();
            return Burndown(model.ProjectId); 
        }
    }
}
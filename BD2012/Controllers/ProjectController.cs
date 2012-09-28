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
            var lineitem = project.LineItems.FirstOrDefault(); 
            var vm = new RowViewModel();
            if (lineitem != null) vm.CopyFrom(lineitem); 
            return View("Burndown", vm); 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBR.Models;

namespace MBR.Web.Controllers
{
    public class RoleController : BaseController
    {
        private MBREntities db = new MBREntities();

        //
        // GET: /Role/

        public ActionResult Index()
        {
            return View(db.Role.ToList());
        }

        //
        // GET: /Role/Details/5

        public ActionResult Details(int id = 0)
        {
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // GET: /Role/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Role/Create

        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                db.Role.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        //
        // GET: /Role/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        //
        // GET: /Role/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Role/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Role.Find(id);
            db.Role.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
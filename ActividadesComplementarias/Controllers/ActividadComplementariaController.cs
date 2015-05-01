using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActividadesComplementarias.Models;

namespace ActividadesComplementarias.Controllers
{
    public class ActividadComplementariaController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /ActividadComplementaria/

        public ActionResult Index()
        {
            var actividadcomplementaria = db.ActividadComplementaria.Include(a => a.Carrera1);
            return View(actividadcomplementaria.ToList());
        }

        //
        // GET: /ActividadComplementaria/Details/5

        public ActionResult Details(int id = 0)
        {
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActividadComplementaria/Create

        public ActionResult Create()
        {
            ViewBag.carrera = new SelectList(db.Carrera, "idCarrera", "nombreCarrera");
            return View();
        }

        //
        // POST: /ActividadComplementaria/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActividadComplementaria actividadcomplementaria)
        {
            if (ModelState.IsValid)
            {
                db.ActividadComplementaria.Add(actividadcomplementaria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.carrera = new SelectList(db.Carrera, "idCarrera", "nombreCarrera", actividadcomplementaria.carrera);
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActividadComplementaria/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            ViewBag.carrera = new SelectList(db.Carrera, "idCarrera", "nombreCarrera", actividadcomplementaria.carrera);
            return View(actividadcomplementaria);
        }

        //
        // POST: /ActividadComplementaria/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActividadComplementaria actividadcomplementaria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividadcomplementaria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.carrera = new SelectList(db.Carrera, "idCarrera", "nombreCarrera", actividadcomplementaria.carrera);
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActividadComplementaria/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            return View(actividadcomplementaria);
        }

        //
        // POST: /ActividadComplementaria/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            db.ActividadComplementaria.Remove(actividadcomplementaria);
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
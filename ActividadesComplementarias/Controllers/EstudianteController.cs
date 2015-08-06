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
    public class EstudianteController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /Estudiante/

        public ActionResult Index()
        {
            string id = Session["user.id"].ToString();
            Maestros maestro = db.Maestros.Find(id);

            var estudiante = db.Estudiante.Include(e => e.Carrera1);
            var student = estudiante.Where(s => s.Carrera1.departamento == maestro.departamentoMaestro);
            return View(student.ToList());
        }

        //
        // GET: /Estudiante/Create

        public ActionResult Create()
        {
            ViewBag.carrera = new SelectList(db.Carrera, "idCarrera", "nombreCarrera");
            return View();
        }

        //
        // POST: /Estudiante/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Estudiante estudiante)
        {
            estudiante.creditosComplementarios = 0;
            if (ModelState.IsValid)
            {
                db.Estudiante.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.carrera = new SelectList(db.Carrera, "idCarrera", "nombreCarrera", estudiante.carrera);
            return View(estudiante);
        }

        //
        // GET: /Estudiante/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Estudiante estudiante = db.Estudiante.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.carrera = new SelectList(db.Carrera, "idCarrera", "nombreCarrera", estudiante.carrera);
            return View(estudiante);
        }

        //
        // POST: /Estudiante/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.carrera = new SelectList(db.Carrera, "idCarrera", "nombreCarrera", estudiante.carrera);
            return View(estudiante);
        }

        //
        // GET: /Estudiante/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Estudiante estudiante = db.Estudiante.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        //
        // POST: /Estudiante/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estudiante estudiante = db.Estudiante.Find(id);
            db.Estudiante.Remove(estudiante);
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
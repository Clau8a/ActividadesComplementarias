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
    public class InscripcionController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /Inscripcion/

        public ActionResult Index()
        {
            var actividadcursada = db.ActividadCursada.Include(a => a.ActividadComplementaria).Include(a => a.Estudiante).Include(a => a.Maestros);
            return View(actividadcursada.ToList());
        }

        public ActionResult Actividades()
        {
            var actividadcomplementaria = db.ActividadComplementaria.Include(a => a.Carrera1);
            return View(actividadcomplementaria.ToList());
        }
        //
        // GET: /Inscripcion/Details/5

        public ActionResult Details(int id = 0)
        {
            ActividadCursada actividadcursada = db.ActividadCursada.Find(id);
            if (actividadcursada == null)
            {
                return HttpNotFound();
            }
            return View(actividadcursada);
        }

        //
        // GET: /Inscripcion/Create

        public ActionResult Create(int id = 0)
        {
            if (id != 0)
            {
                var actividad = db.ActividadComplementaria.Find(id);
                ViewBag.idActComplementaria = actividad.nombreActComplementaria;
                ViewBag.idEstudiante = Session["uxid"].ToString();
            }
            ViewBag.mestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro");
            return View();
        }

        //
        // POST: /Inscripcion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActividadCursada actividadcursada, string idEstudiante)
        {
            var estudiante= from student in db.Estudiante
                            where student.nombreEstudiante==idEstudiante
                                select student;
            foreach(var item in estudiante)
            {
                actividadcursada.idEstudiante = item.idEstudiante;
            }
            
            if (ModelState.IsValid)
            {
                db.ActividadCursada.Add(actividadcursada);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.mestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcursada.mestro);
            return View(actividadcursada);
        }

        //
        // GET: /Inscripcion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ActividadCursada actividadcursada = db.ActividadCursada.Find(id);
            if (actividadcursada == null)
            {
                return HttpNotFound();
            }
            ViewBag.idActComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", actividadcursada.idActComplementaria);
            ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", actividadcursada.idEstudiante);
            ViewBag.mestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcursada.mestro);
            return View(actividadcursada);
        }

        //
        // POST: /Inscripcion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActividadCursada actividadcursada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividadcursada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idActComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", actividadcursada.idActComplementaria);
            ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", actividadcursada.idEstudiante);
            ViewBag.mestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcursada.mestro);
            return View(actividadcursada);
        }

        //
        // GET: /Inscripcion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ActividadCursada actividadcursada = db.ActividadCursada.Find(id);
            if (actividadcursada == null)
            {
                return HttpNotFound();
            }
            return View(actividadcursada);
        }

        //
        // POST: /Inscripcion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActividadCursada actividadcursada = db.ActividadCursada.Find(id);
            db.ActividadCursada.Remove(actividadcursada);
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
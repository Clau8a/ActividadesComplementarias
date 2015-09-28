using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActividadesComplementarias.Models;

namespace ActividadesComplementariasControllers
{
    public class ActividadCursadaController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /ActividadCursada/

        public ActionResult Index(int id=0)
        {
            if (id != 0)
            {
                Estudiante stu = db.Estudiante.Find(id);
                var actividadcursada = db.ActividadCursada.Include(a => a.Estudiante).Include(a => a.Grupos);
                var ac = actividadcursada.Where(a => a.idEstudiante == id);
                return View(ac.ToList());

            }
            else
            {
                var actividadcursada = db.ActividadCursada.Include(a => a.Estudiante).Include(a => a.Grupos);
                return View(actividadcursada.ToList());
            }
        }

        //
        // GET: /ActividadCursada/Details/5

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
        // GET: /ActividadCursada/Create

        public ActionResult Create()
        {
            ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante");
            ViewBag.idGrupo = new SelectList(db.Grupos, "idGrupo", "nombreGrupo");
            return View();
        }

        //
        // POST: /ActividadCursada/Create

        [HttpPost]
        public ActionResult Create(ActividadCursada actividadcursada)
        {
            if (ModelState.IsValid)
            {
                db.ActividadCursada.Add(actividadcursada);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", actividadcursada.idEstudiante);
            ViewBag.idGrupo = new SelectList(db.Grupos, "idGrupo", "nombreGrupo", actividadcursada.idGrupo);
            return View(actividadcursada);
        }

        //
        // GET: /ActividadCursada/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ActividadCursada actividadcursada = db.ActividadCursada.Find(id);
            if (actividadcursada == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", actividadcursada.idEstudiante);
            ViewBag.idGrupo = new SelectList(db.Grupos, "idGrupo", "nombreGrupo", actividadcursada.idGrupo);
            return View(actividadcursada);
        }

        //
        // POST: /ActividadCursada/Edit/5

        [HttpPost]
        public ActionResult Edit(ActividadCursada actividadcursada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividadcursada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", actividadcursada.idEstudiante);
            ViewBag.idGrupo = new SelectList(db.Grupos, "idGrupo", "nombreGrupo", actividadcursada.idGrupo);
            return View(actividadcursada);
        }

        //
        // GET: /ActividadCursada/Delete/5

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
        // POST: /ActividadCursada/Delete/5

        [HttpPost, ActionName("Delete")]
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
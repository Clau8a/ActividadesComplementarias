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
            string id = Session["user.id"].ToString();
            Maestros maestro = db.Maestros.Find(id);
            var actividadcomplementaria = db.ActividadComplementaria.Include(a => a.Departamento1).Include(a => a.Maestros);
            var actCom = actividadcomplementaria.Where(s => s.Departamento1.idDepartamento == maestro.departamentoMaestro || s.departamento == 123457);
            return View(actCom.ToList());
        }

        public ActionResult List(int id=0)
        {
            //Carrera y departamento filtrar y ver como regresar a los estudiantes 
            var acti= db.ActividadCursada.Include(a=> a.Estudiante);
            var acticur = acti.Where(a => a.idActComplementaria == id);

            
            var actcomp = db.ActividadComplementaria.Find(id);
            ViewBag.Actividad = actcomp.nombreActComplementaria;

            return View(acticur.ToList());
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
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento");
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro");
            return View();
        }

        //
        // POST: /ActividadComplementaria/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActividadComplementaria actividadcomplementaria)
        {
            actividadcomplementaria.idActividadComplementaria = 0;
            if (ModelState.IsValid)
            {
                db.ActividadComplementaria.Add(actividadcomplementaria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.carrera = new SelectList(db.Carrera, "idDepartamento", "nombreCarrera", actividadcomplementaria.departamento);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcomplementaria.maestro);
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
            //ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento");
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", actividadcomplementaria.departamento);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcomplementaria.maestro);
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
            ViewBag.carrera = new SelectList(db.Carrera, "idDepartamento", "nombreCarrera", actividadcomplementaria.departamento);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcomplementaria.maestro);
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
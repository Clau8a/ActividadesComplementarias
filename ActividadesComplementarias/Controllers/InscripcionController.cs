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

        public ActionResult Index(int id=0)
        {
            if (id != 0)
            { 
                var cursadas= from cur in db.ActividadCursada
                              where cur.idEstudiante==id
                                  select cur;
                var curs =cursadas.Include(a => a.ActividadComplementaria).Include(a => a.Estudiante).Include(a => a.Maestros);
                return View(curs.ToList());
            }
            else
            {
                var actividadcursada = db.ActividadCursada.Include(a => a.ActividadComplementaria).Include(a => a.Estudiante).Include(a => a.Maestros);
                return View(actividadcursada.ToList());
            }
        }

        public ActionResult Actividades(int id=0)
        {
            Estudiante student = db.Estudiante.Find(id);
            if (id != 0) 
            {
                var actividades= from ac in db.ActividadComplementaria
                                 where ac.Departamento1.idDepartamento == student.Carrera1.departamento || ac.departamento == 123457
                                     select ac;
                var filtro = actividades.Include(a => a.Departamento1);
                return View(filtro.ToList());
            }
            else 
            { 
                var actividadcomplementaria = db.ActividadComplementaria.Include(a => a.Departamento1);
                return View(actividadcomplementaria.ToList());
            }
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
                ViewBag.periodo=CalculaPeriodo();
                var actividad = db.ActividadComplementaria.Find(id);
                ViewBag.idActComplementaria = actividad.nombreActComplementaria;
                ViewBag.idActComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria",id);
                string student1=Session["uxid"].ToString();
                var estudiante = from student in db.Estudiante
                                 where student.nombreEstudiante == student1
                                 select student;
                foreach (var item in estudiante)
                {
                    ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", item.idEstudiante);
                }
            }
            ViewBag.mestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro");
            return View();
        }

        //
        // POST: /Inscripcion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActividadCursada actividadcursada)
        {
            actividadcursada.estatusActividad = "Cursando";
            if (ModelState.IsValid)
            {
                db.ActividadCursada.Add(actividadcursada);
                db.SaveChanges();
                return Redirect("/Inscripcion/Index/"+actividadcursada.idEstudiante);
            }

            ViewBag.mestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcursada.mestro);
            return View(actividadcursada);
        }

        public ActionResult Acreditar(int id = 0)
        {
            ActividadCursada actividadcursada = db.ActividadCursada.Find(id);
            actividadcursada.estatusActividad = "Acreditada";
            db.Entry(actividadcursada).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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

        public string CalculaPeriodo() 
        {
            var perioAño = DateTime.Today.Year;
            int month=DateTime.Today.Month;
            string per="";
            if (month <= 6)
                per = "-1";
            else
                per = "-2";
            string periodo = perioAño + per;
            return periodo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
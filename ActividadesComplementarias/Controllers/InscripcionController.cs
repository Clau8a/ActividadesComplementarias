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
                Estudiante es= db.Estudiante.Find(id);
                var cursadas= from cur in db.ActividadCursada
                              where cur.idEstudiante==id
                                  select cur;
                var curs =cursadas.Include(a => a.Grupos.ActividadComplementaria1).Include(a => a.Estudiante)/*.Include(a => a.Maestros)*/;
                ViewBag.TotalCreditos = es.creditosComplementarios;
                return View(curs.ToList());
            }
            else
            {
                string id1 = Session["user.id"].ToString();
                Maestros maestro = db.Maestros.Find(id1);
                var actividadcursada = db.ActividadCursada.Include(a => a.Grupos.ActividadComplementaria1).Include(a => a.Estudiante)/*.Include(a => a.Maestros)*/;
                var inscritos = actividadcursada.Where(s => s.Grupos.ActividadComplementaria1.Departamento1.idDepartamento == maestro.departamentoMaestro || s.Grupos.ActividadComplementaria1.Departamento1.idDepartamento == 123457);
                return View(inscritos.ToList());
            }
        }

        public ActionResult Actividades(int id=0)
        {
            Estudiante student = db.Estudiante.Find(id);
            if (id != 0) 
            {
                var actividades= from ac in db.ActividadComplementaria
                                 where ac.Departamento1.idDepartamento == student.Carrera1.departamento || ac.departamento == 123457 || ac.departamento == 123459
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
                    //ViewBag.idActComplementaria = //new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria",id);
                    string student1=Session["uxid"].ToString();
                    var estudiante = from student in db.Estudiante
                                        where student.nombreEstudiante == student1
                                        select student;
                    foreach (var item in estudiante)
                    {
                        ViewBag.idEstudiante = item.nombreEstudiante;//new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", item.idEstudiante);
                    }
                    //var maestro = from teacher in db.Maestros
                    //                    where teacher.idMaestro == actividad.
                    //                    select teacher;
                    //foreach (var item in maestro) 
                    //{
                    //    ViewBag.mestro = item.nombreMaestro;//new SelectList(db.Maestros, "idMaestro", "nombreMaestro",item.idMaestro);
                    //}
                
            }
            
            return View();
        }

        //
        // POST: /Inscripcion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActividadCursada actividadcursada,string student,string actividad)
        {
            actividadcursada.idAvtividadCursada = 0;
            actividadcursada.estatusActividad = "Cursando";

            var ac = from act in db.ActividadComplementaria
                     where act.nombreActComplementaria == actividad
                     select act;
            foreach (var item in ac)
            {
                actividadcursada.Grupos.actividadComplementaria = item.idActividadComplementaria;
            }

            Grupos grupoAC = db.Grupos.Find(actividadcursada.idGrupo);
            
                var stu = from s in db.Estudiante
                          where s.nombreEstudiante == student
                          select s;
                foreach (var item in stu)
                {
                    actividadcursada.idEstudiante = item.idEstudiante;
                }


                var ma = from m in db.Maestros
                         where m.nombreMaestro == actividadcursada.Grupos.maestro
                         select m;
                foreach (var item in ma)
                {
                    actividadcursada.Grupos.maestro = item.idMaestro;
                }

                if (ModelState.IsValid)
                {
                    grupoAC.inscritos++;
                    db.Entry(grupoAC).State = EntityState.Modified;
                    db.ActividadCursada.Add(actividadcursada);
                    db.SaveChanges();
                    return Redirect("/Inscripcion/Index/" + actividadcursada.idEstudiante);
                }
            

            
            return View(actividadcursada);
        }

        public ActionResult Acreditar(int id = 0)
        {
            //sumar los creditos
            ActividadCursada actividadcursada = db.ActividadCursada.Find(id);
            actividadcursada.estatusActividad = "Acreditada";
            db.Entry(actividadcursada).State = EntityState.Modified;
            ActividadComplementaria ac = db.ActividadComplementaria.Find(actividadcursada.Grupos.actividadComplementaria);
            Estudiante es = db.Estudiante.Find(actividadcursada.idEstudiante);
            if ((es.creditosComplementarios + ac.noCreditos) > 5)
                es.creditosComplementarios = 5;
            else
            es.creditosComplementarios += ac.noCreditos;
            db.Entry(es).State = EntityState.Modified;

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
            ViewBag.idActComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", actividadcursada.Grupos.actividadComplementaria);
            ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", actividadcursada.idEstudiante);
            ViewBag.mestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcursada.Grupos.maestro);
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
            ViewBag.idActComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", actividadcursada.Grupos.actividadComplementaria);
            ViewBag.idEstudiante = new SelectList(db.Estudiante, "idEstudiante", "nombreEstudiante", actividadcursada.idEstudiante);
            ViewBag.mestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcursada.Grupos.maestro);
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
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
    public class ActividadComplementariaController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /ActiviadesComplementarias/

        public ActionResult Index()
        {
            if (Session["user.id"] == null) 
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            Maestros mae = db.Maestros.Find(Session["user.id"].ToString());
            var actividadcomplementaria = db.ActividadComplementaria.Include(a => a.Departamento1);
            if (mae.departamentoMaestro == 123457)
            {
                return View(actividadcomplementaria.ToList());
            }
            else
            {
                var act = actividadcomplementaria.Where(a => a.departamento == mae.departamentoMaestro);
                return View(act.ToList());
            }
        }

        //
        // GET: /ActiviadesComplementarias/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActiviadesComplementarias/Create

        public ActionResult Create()
        {
           
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento");
            return View();
        }

        //
        // POST: /ActiviadesComplementarias/Create

        [HttpPost]
        public ActionResult Create(ActividadComplementaria actividadcomplementaria)
        {
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            if (ModelState.IsValid)
            {
                db.ActividadComplementaria.Add(actividadcomplementaria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", actividadcomplementaria.departamento);
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActiviadesComplementarias/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", actividadcomplementaria.departamento);
            return View(actividadcomplementaria);
        }

        //
        // POST: /ActiviadesComplementarias/Edit/5

        [HttpPost]
        public ActionResult Edit(ActividadComplementaria actividadcomplementaria)
        {
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(actividadcomplementaria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", actividadcomplementaria.departamento);
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActiviadesComplementarias/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            return View(actividadcomplementaria);
        }

        //
        // POST: /ActiviadesComplementarias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            db.ActividadComplementaria.Remove(actividadcomplementaria);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Inscripciones(int id) 
        {
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            Estudiante stu= db.Estudiante.Find(id);
            var actividadcomplementaria = db.ActividadComplementaria.Include(a => a.Departamento1);
            var ac = actividadcomplementaria.Where(a => a.departamento == stu.Carrera1.departamento || a.departamento == 123459);
            return View(actividadcomplementaria.ToList());
        }
       

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
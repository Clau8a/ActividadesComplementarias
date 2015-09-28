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
    public class CarreraController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /Carrera/

        public ActionResult Index()
        {
            var carrera = db.Carrera.Include(c => c.Departamento1);
            return View(carrera.ToList());
        }

        //
        // GET: /Carrera/Details/5

        public ActionResult Details(int id = 0)
        {
            Carrera carrera = db.Carrera.Find(id);
            if (carrera == null)
            {
                return HttpNotFound();
            }
            return View(carrera);
        }

        //
        // GET: /Carrera/Create

        public ActionResult Create()
        {
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento");
            return View();
        }

        //
        // POST: /Carrera/Create

        [HttpPost]
        public ActionResult Create(Carrera carrera)
        {
            if (ModelState.IsValid)
            {
                db.Carrera.Add(carrera);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", carrera.departamento);
            return View(carrera);
        }

        //
        // GET: /Carrera/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Carrera carrera = db.Carrera.Find(id);
            if (carrera == null)
            {
                return HttpNotFound();
            }
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", carrera.departamento);
            return View(carrera);
        }

        //
        // POST: /Carrera/Edit/5

        [HttpPost]
        public ActionResult Edit(Carrera carrera)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carrera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", carrera.departamento);
            return View(carrera);
        }

        //
        // GET: /Carrera/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Carrera carrera = db.Carrera.Find(id);
            if (carrera == null)
            {
                return HttpNotFound();
            }
            return View(carrera);
        }

        //
        // POST: /Carrera/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Carrera carrera = db.Carrera.Find(id);
            db.Carrera.Remove(carrera);
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
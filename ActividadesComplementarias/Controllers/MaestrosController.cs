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
    public class MaestrosController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /Maestros/

        public ActionResult Index()
        {
            var maestros = db.Maestros.Include(m => m.Departamento);
            return View(maestros.ToList());
        }

        //
        // GET: /Maestros/Details/5

        public ActionResult Details(int id = 0)
        {
            Maestros maestros = db.Maestros.Find(id);
            if (maestros == null)
            {
                return HttpNotFound();
            }
            return View(maestros);
        }

        //
        // GET: /Maestros/Create

        public ActionResult Create()
        {
            ViewBag.departamentoMaestro = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento");
            return View();
        }

        //
        // POST: /Maestros/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Maestros maestros)
        {
            if (ModelState.IsValid)
            {
                db.Maestros.Add(maestros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.departamentoMaestro = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", maestros.departamentoMaestro);
            return View(maestros);
        }

        //
        // GET: /Maestros/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Maestros maestros = db.Maestros.Find(id);
            if (maestros == null)
            {
                return HttpNotFound();
            }
            ViewBag.departamentoMaestro = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", maestros.departamentoMaestro);
            return View(maestros);
        }

        //
        // POST: /Maestros/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Maestros maestros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maestros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departamentoMaestro = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", maestros.departamentoMaestro);
            return View(maestros);
        }

        //
        // GET: /Maestros/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Maestros maestros = db.Maestros.Find(id);
            if (maestros == null)
            {
                return HttpNotFound();
            }
            return View(maestros);
        }

        //
        // POST: /Maestros/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maestros maestros = db.Maestros.Find(id);
            db.Maestros.Remove(maestros);
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
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
    public class DepartamentoController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /Departamento/

        public ActionResult Index()
        {
            return View(db.Departamento.ToList());
        }

        //
        // GET: /Departamento/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Departamento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                Departamento deptoExistente = db.Departamento.Find(departamento.idDepartamento);
                if (deptoExistente == null)
                {
                    db.Departamento.Add(departamento);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else 
                {
                    return View(departamento);
                }
            }

            return View(departamento);
        }

        //
        // GET: /Departamento/Edit/5

        public ActionResult Edit(int id)
        {
            Departamento departamento = db.Departamento.Find(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        //
        // POST: /Departamento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departamento);
        }

        //
        // GET: /Departamento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Departamento departamento = db.Departamento.Find(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        //
        // POST: /Departamento/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Departamento departamento = db.Departamento.Find(id);
            db.Departamento.Remove(departamento);
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
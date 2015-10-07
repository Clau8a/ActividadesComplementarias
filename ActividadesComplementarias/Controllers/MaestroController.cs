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
    public class MaestroController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /Maestro/

        public ActionResult Index()
        {
            var maestros = db.Maestros.Include(m => m.Departamento).Include(m => m.TipoMaestro1);
            return View(maestros.ToList());
        }

        //
        // GET: /Maestro/Details/5

        public ActionResult Details(string id = null)
        {
            Maestros maestros = db.Maestros.Find(id);
            if (maestros == null)
            {
                return HttpNotFound();
            }
            return View(maestros);
        }

        //
        // GET: /Maestro/Create

        public ActionResult Create()
        {
            ViewBag.departamentoMaestro = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento");
            ViewBag.tipoMaestro = new SelectList(db.TipoMaestro, "idTipoMaestro", "tipoMaestro1");
            return View();
        }

        //
        // POST: /Maestro/Create

        [HttpPost]
        public ActionResult Create(Maestros maestros)
        {
            if (ModelState.IsValid)
            {
                db.Maestros.Add(maestros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.departamentoMaestro = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", maestros.departamentoMaestro);
            ViewBag.tipoMaestro = new SelectList(db.TipoMaestro, "idTipoMaestro", "tipoMaestro1", maestros.tipoMaestro);
            return View(maestros);
        }

        //
        // GET: /Maestro/Edit/5

        public ActionResult Edit(string id = null)
        {
            Maestros maestros = db.Maestros.Find(id);
            if (maestros == null)
            {
                return HttpNotFound();
            }
            ViewBag.departamentoMaestro = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", maestros.departamentoMaestro);
            ViewBag.tipoMaestro = new SelectList(db.TipoMaestro, "idTipoMaestro", "tipoMaestro1", maestros.tipoMaestro);
            return View(maestros);
        }

        //
        // POST: /Maestro/Edit/5

        [HttpPost]
        public ActionResult Edit(Maestros maestros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maestros).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/ActividadComplementaria");
            }
            ViewBag.departamentoMaestro = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", maestros.departamentoMaestro);
            ViewBag.tipoMaestro = new SelectList(db.TipoMaestro, "idTipoMaestro", "tipoMaestro1", maestros.tipoMaestro);
            return View(maestros);
        }

        //
        // GET: /Maestro/Delete/5

        public ActionResult Delete(string id = null)
        {
            Maestros maestros = db.Maestros.Find(id);
            if (maestros == null)
            {
                return HttpNotFound();
            }
            return View(maestros);
        }

        //
        // POST: /Maestro/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
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
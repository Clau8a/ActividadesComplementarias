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
    public class grurupoController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /grurupo/

        public ActionResult Index()
        {
            var grupos = db.Grupos.Include(g => g.ActividadComplementaria1).Include(g => g.Maestros);
            return View(grupos.ToList());
        }

        //
        // GET: /grurupo/Details/5

        public ActionResult Details(int id = 0)
        {
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            return View(grupos);
        }

        //
        // GET: /grurupo/Create

        public ActionResult Create()
        {
            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria");
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro");
            return View();
        }

        //
        // POST: /grurupo/Create

        [HttpPost]
        public ActionResult Create(Grupos grupos)
        {
            if (ModelState.IsValid)
            {
                db.Grupos.Add(grupos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", grupos.actividadComplementaria);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", grupos.maestro);
            return View(grupos);
        }

        //
        // GET: /grurupo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", grupos.actividadComplementaria);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", grupos.maestro);
            return View(grupos);
        }

        //
        // POST: /grurupo/Edit/5

        [HttpPost]
        public ActionResult Edit(Grupos grupos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", grupos.actividadComplementaria);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", grupos.maestro);
            return View(grupos);
        }

        //
        // GET: /grurupo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            return View(grupos);
        }

        //
        // POST: /grurupo/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Grupos grupos = db.Grupos.Find(id);
            db.Grupos.Remove(grupos);
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
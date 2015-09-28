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
    public class GruposController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /Grupos/

        public ActionResult Index(int id=0)
        {
            ViewBag.idActComple = id;
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            Maestros mae = db.Maestros.Find(Session["user.id"].ToString());
            var actividadcomplementaria = db.ActividadComplementaria.Include(a => a.Departamento1);
            if (mae.departamentoMaestro == 123457)
            {
                var grups = db.Grupos.Include(g => g.ActividadComplementaria1);
                return View(grups.ToList());
            }
            else
            {
                
                var grupos = db.Grupos.Include(g => g.ActividadComplementaria1).Where(g => g.actividadComplementaria == id);
                return View(grupos.ToList());
            }
        }

        //
        // GET: /Grupos/Details/5


        //
        // GET: /Grupos/Create

        public ActionResult Create(int id=0)
        {
            ViewBag.idActComple = id;
            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria");
            return View();
        }

        //
        // POST: /Grupos/Create

        [HttpPost]
        public ActionResult Create(Grupos grupos)
        {
            grupos.idGrupo = 0;
            grupos.inscritos = 0;
            
            if (ModelState.IsValid)
            {
                db.Grupos.Add(grupos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", grupos.actividadComplementaria);
            return View(grupos);
        }

        //
        // GET: /Grupos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", grupos.actividadComplementaria);
            return View(grupos);
        }

        //
        // POST: /Grupos/Edit/5

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
            return View(grupos);
        }

        //
        // GET: /Grupos/Delete/5

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
        // POST: /Grupos/Delete/5

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
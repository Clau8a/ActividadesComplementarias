using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActividadesComplementarias.Models;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System.IO;

namespace ActividadesComplementarias.Controllers
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

        public ActionResult Lista(int id=0) 
        {
            if (Session["user.id"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            var grupos = db.Grupos.Include(g => g.ActividadComplementaria1).Where(g => g.actividadComplementaria == id);
            return View(grupos.ToList());
        }

        public ActionResult Inscripcion(int id=0) 
        {
            int idEstu = Convert.ToInt32(Session["user.id"].ToString());
            
            ActividadCursada actCur = new ActividadCursada();
            actCur.idAvtividadCursada = 0;
            actCur.idGrupo = id;
            actCur.estatusActividad = "Cursando";
            actCur.periodo = CalculaPeriodo();
            actCur.idEstudiante = idEstu;

            Grupos grup = db.Grupos.Find(id);

            if (ModelState.IsValid)
            {
                db.ActividadCursada.Add(actCur);
                grup.inscritos += 1;
                db.Entry(grup).State = EntityState.Modified;
                db.SaveChanges();

                return Content("success", "text/plain");
                //return Redirect("/ActividadCursada/Index/"+actCur.idEstudiante);
            }

            return Redirect("");
        }


        public ActionResult Inscritos(int id=0) 
        {
            Grupos gru=db.Grupos.Find(id);
            ViewBag.idActCompl = gru.idGrupo;
            var actividadcursada = db.ActividadCursada.Include(a => a.Estudiante).Include(a => a.Grupos).Where(a=>a.idGrupo==id);
            return View(actividadcursada.ToList());
        }

        //
        // GET: /Grupos/Create

        public ActionResult Create(int id=0)
        {
            ViewBag.idActComple = id;
            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria");
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro");
            return View();
        }

        //
        // POST: /Grupos/Create

        [HttpPost]
        public ActionResult Create(Grupos grupos, string val=null)
        {
            grupos.idGrupo = 0;
            grupos.inscritos = 0;
            ViewBag.idActComple = grupos.actividadComplementaria;
            if (ModelState.IsValid)
            {
                db.Grupos.Add(grupos);
                db.SaveChanges();
                return Redirect("/Grupos/Index/"+grupos.actividadComplementaria);
            }

            ViewBag.actividadComplementaria = new SelectList(db.ActividadComplementaria, "idActividadComplementaria", "nombreActComplementaria", grupos.actividadComplementaria);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", grupos.maestro);
            return Redirect("/Grupos/Index/"+grupos.actividadComplementaria);
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
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", grupos.maestro);
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
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", grupos.maestro);
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



        public ActionResult Acreditar(int id=0) 
        {
            ActividadCursada actCur = db.ActividadCursada.Find(id);
            actCur.estatusActividad = "Acreditada";
            Estudiante estu = db.Estudiante.Find(actCur.idEstudiante);
            estu.creditosComplementarios += actCur.Grupos.ActividadComplementaria1.noCreditos;
            if (estu.creditosComplementarios>5)
            {
                estu.creditosComplementarios = 5;
            }
            if (ModelState.IsValid)
            {
                db.Entry(estu).State = EntityState.Modified;
                db.Entry(actCur).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Grupos/Inscritos/"+actCur.idGrupo);
            }
            return Redirect("/Grupos/Inscritos/" + actCur.idGrupo);
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

        public ActionResult printReportList(int id = 0, string ext = null, string periodo = null)
        {
            Grupos grupo = db.Grupos.Find(id);
            LocalReport lr = new LocalReport();
            string pathReport = Path.Combine(Server.MapPath("~/Resources"), "ListasPorActividad.rdlc");
            lr.ReportPath = pathReport;

            ReportParameter[] parametro = new ReportParameter[3];
            parametro[0] = new ReportParameter("ac", grupo.ActividadComplementaria1.nombreActComplementaria);
            if (Session["user.tipo"].ToString() == "X" || Session["user.tipo"].ToString() == "D")
            {
                periodo = CalculaPeriodo();
                parametro[1] = new ReportParameter("periodo", periodo);
            }
            else
            {
                parametro[1] = new ReportParameter("periodo", periodo);
            }
            parametro[2] = new ReportParameter("maestro", grupo.Maestros.nombreMaestro);

            lr.SetParameters(parametro);

            string json = JsonConvert.SerializeObject(db.lst_byact(id, periodo));


            DataTable dtDetalleTF = JsonConvert.DeserializeObject<DataTable>(json);
            DataSet DSDetalleTF = new DataSet();
            DSDetalleTF.Tables.Add(dtDetalleTF);
            ReportDataSource rdtsDetalleTF = new ReportDataSource();
            rdtsDetalleTF.Name = "DSListas";
            rdtsDetalleTF.Value = DSDetalleTF.Tables[0];
            lr.DataSources.Add(rdtsDetalleTF);
            if (ext == "Excel")
            {
                Response.AddHeader("content-disposition", "attachment;    filename=DetalleTrabajo" + id + " -" + DateTime.Today.ToShortDateString() + ".xls");
            }
            string reportType = ext;
            string mimeType;
            string encoding;
            string fileNameExtension;
            //string deviceInfo = "<DeviceInfo>" + "  <OutputFormat>" + reportType + "</OutputFormat>" + "  <PageWidth>8.5in</PageWidth>" + "  <PageHeight>11in</PageHeight>" + "  <MarginTop>0.5in</MarginTop>" + "  <MarginLeft>1in</MarginLeft>" + "  <MarginRight>1in</MarginRight>" + "  <MarginBottom>0.5in</MarginBottom>" + "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = lr.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public ActionResult printListAsistencia(int id = 0, string ext = null, string periodo = null)
        {
            Grupos grupo = db.Grupos.Find(id);
            LocalReport lr = new LocalReport();
            string pathReport = Path.Combine(Server.MapPath("~/Resources"), "ListasAsistencia.rdlc");
            lr.ReportPath = pathReport;

            ReportParameter[] parametro = new ReportParameter[3];
            parametro[0] = new ReportParameter("ac", grupo.ActividadComplementaria1.nombreActComplementaria);
            if (Session["user.tipo"].ToString() == "X" || Session["user.tipo"].ToString() == "D")
            {
                periodo = CalculaPeriodo();
                parametro[1] = new ReportParameter("periodo", periodo);
            }
            else
            {
                parametro[1] = new ReportParameter("periodo", periodo);
            }
            parametro[2] = new ReportParameter("maestro", grupo.Maestros.nombreMaestro);

            lr.SetParameters(parametro);

            string json = JsonConvert.SerializeObject(db.listAsistencia(id, periodo));


            DataTable dtDetalleTF = JsonConvert.DeserializeObject<DataTable>(json);
            DataSet DSDetalleTF = new DataSet();
            DSDetalleTF.Tables.Add(dtDetalleTF);
            ReportDataSource rdtsDetalleTF = new ReportDataSource();
            rdtsDetalleTF.Name = "DSAsistencia";
            rdtsDetalleTF.Value = DSDetalleTF.Tables[0];
            lr.DataSources.Add(rdtsDetalleTF);

            if (ext == "Excel")
            {
                Response.AddHeader("content-disposition", "attachment;    filename=DetalleTrabajo" + id + " -" + DateTime.Today.ToShortDateString() + ".xls");
            }
            string reportType = ext;
            string mimeType;
            string encoding;
            string fileNameExtension;
            //string deviceInfo = "<DeviceInfo>" + "  <OutputFormat>" + reportType + "</OutputFormat>" + "  <PageWidth>8.5in</PageWidth>" + "  <PageHeight>11in</PageHeight>" + "  <MarginTop>0.5in</MarginTop>" + "  <MarginLeft>1in</MarginLeft>" + "  <MarginRight>1in</MarginRight>" + "  <MarginBottom>0.5in</MarginBottom>" + "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = lr.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public string CalculaPeriodo()
        {
            var perioAño = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            string per = "";
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
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
    public class ActividadComplementariaController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();

        //
        // GET: /ActividadComplementaria/

        public ActionResult Index()
        {
            
            string id = Session["user.id"].ToString();
            Maestros maestro = db.Maestros.Find(id);
            var actividadcomplementaria = db.ActividadComplementaria.Include(a => a.Departamento1).Include(a => a.Maestros);
            if (Session["user.tipo"].ToString()=="X") 
            {
                var actCom = actividadcomplementaria.Where(s => s.modalidad == "Extraescolar");
                return View(actCom.ToList());
            }
            else{
                if (Session["user.tipo"].ToString() == "J") 
                {
                    return View(actividadcomplementaria.ToList());
                }
                else 
                { 
                    var actCom = actividadcomplementaria.Where( s => s.Departamento1.idDepartamento == maestro.departamentoMaestro || s.departamento == 123457 );
                    return View(actCom.ToList());
                }
            }
        }

        public ActionResult List(int id = 0, string periodoFiltro = null)
        {
            //Carrera y departamento filtrar y ver como regresar a los estudiantes 
            var acti= db.ActividadCursada.Include(a=> a.Estudiante);
            var acticur = acti.Where(a => a.idActComplementaria == id);

            if (Session["user.tipo"].ToString() == "X" || Session["user.tipo"].ToString() == "D") 
            {
                string periodos = CalculaPeriodo();
                acticur = acti.Where(a => a.idActComplementaria == id && a.periodo == periodos);
            }
            
                if (periodoFiltro != null)
                {
                    acticur = acti.Where(a => a.idActComplementaria == id && a.periodo == periodoFiltro);
                }
            
            ViewBag.periodo =new SelectList(db.slctPeriodo());
            
            var actcomp = db.ActividadComplementaria.Find(id);
            ViewBag.Actividad = actcomp.nombreActComplementaria;
            ViewBag.idActCompl = id;
            return View(acticur.ToList());
        }

        public ActionResult Acreditar(int id = 0)
        {
            //sumar los creditos
            ActividadCursada actividadcursada = db.ActividadCursada.Find(id);
            actividadcursada.estatusActividad = "Acreditada";
            db.Entry(actividadcursada).State = EntityState.Modified;
            ActividadComplementaria ac = db.ActividadComplementaria.Find(actividadcursada.idActComplementaria);
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
        // GET: /ActividadComplementaria/Details/5

        public ActionResult Details(int id = 0)
        {
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActividadComplementaria/Create

        public ActionResult Create()
        {
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento");
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro");
            

            return View();
        }

        //
        // POST: /ActividadComplementaria/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActividadComplementaria actividadcomplementaria)
        {
            actividadcomplementaria.idActividadComplementaria = 0;
            if (ModelState.IsValid)
            {
                db.ActividadComplementaria.Add(actividadcomplementaria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.carrera = new SelectList(db.Carrera, "idDepartamento", "nombreCarrera", actividadcomplementaria.departamento);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcomplementaria.maestro);
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActividadComplementaria/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            //ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento");
            ViewBag.departamento = new SelectList(db.Departamento, "idDepartamento", "nombreDepartamento", actividadcomplementaria.departamento);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcomplementaria.maestro);
            return View(actividadcomplementaria);
        }

        //
        // POST: /ActividadComplementaria/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActividadComplementaria actividadcomplementaria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividadcomplementaria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.carrera = new SelectList(db.Carrera, "idDepartamento", "nombreCarrera", actividadcomplementaria.departamento);
            ViewBag.maestro = new SelectList(db.Maestros, "idMaestro", "nombreMaestro", actividadcomplementaria.maestro);
            return View(actividadcomplementaria);
        }

        //
        // GET: /ActividadComplementaria/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            if (actividadcomplementaria == null)
            {
                return HttpNotFound();
            }
            return View(actividadcomplementaria);
        }

        //
        // POST: /ActividadComplementaria/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActividadComplementaria actividadcomplementaria = db.ActividadComplementaria.Find(id);
            db.ActividadComplementaria.Remove(actividadcomplementaria);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult printReportList(int id = 0, string ext = null,string periodo=null)
        {
            ActividadComplementaria ac = db.ActividadComplementaria.Find(id);
            LocalReport lr = new LocalReport();
            string pathReport = Path.Combine(Server.MapPath("~/Resources"), "ListasPorActividad.rdlc");
            lr.ReportPath = pathReport;

            ReportParameter[] parametro = new ReportParameter[3];
            parametro[0] = new ReportParameter("ac", ac.nombreActComplementaria);
            if (Session["user.tipo"].ToString() == "X" || Session["user.tipo"].ToString() == "D")
            {
                periodo = CalculaPeriodo();
                parametro[1] = new ReportParameter("periodo", periodo);
            }
            else
            {
                parametro[1] = new ReportParameter("periodo", periodo);
            }
            parametro[2] = new ReportParameter("maestro", ac.Maestros.nombreMaestro);

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
            ActividadComplementaria ac = db.ActividadComplementaria.Find(id);
            LocalReport lr = new LocalReport();
            string pathReport = Path.Combine(Server.MapPath("~/Resources"), "ListasAsistencia.rdlc");
            lr.ReportPath = pathReport;

            ReportParameter[] parametro = new ReportParameter[3];
            parametro[0] = new ReportParameter("ac", ac.nombreActComplementaria);
            if (Session["user.tipo"].ToString() == "X" || Session["user.tipo"].ToString() == "D")
            {
                periodo = CalculaPeriodo();
                parametro[1] = new ReportParameter("periodo", periodo);
            }
            else
            {
                parametro[1] = new ReportParameter("periodo", periodo);
            }
            parametro[2] = new ReportParameter("maestro", ac.Maestros.nombreMaestro);

            lr.SetParameters(parametro);

            string json = JsonConvert.SerializeObject(db.listAsistencia(id,periodo));


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
    }


}
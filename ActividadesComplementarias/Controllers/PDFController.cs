using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using ActividadesComplementarias.Models;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;

namespace ActividadesComplementarias.Controllers
{
    public class PDFController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();
        //
        // GET: /PDF/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cola(int id,string periodo=null) // id = num_certificado_nac
        {
            ActividadComplementaria actCursada = db.ActividadComplementaria.Find(id);

            string idJefe= Session["user.id"].ToString();
            Maestros ma= db.Maestros.Find(idJefe);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Resources"), "Constancia.rdlc");
            lr.ReportPath = path;

            ReportParameter[] parametro = new ReportParameter[7];
            parametro[0] = new ReportParameter("departamento", actCursada.Departamento1.nombreDepartamento);
            parametro[1] = new ReportParameter("jd", ma.nombreMaestro);
            parametro[2] = new ReportParameter("creditos", actCursada.noCreditos.ToString());
            parametro[3] = new ReportParameter("ac", actCursada.nombreActComplementaria);
            parametro[4] = new ReportParameter("dia", DateTime.Today.Day.ToString());
            parametro[5] = new ReportParameter("mes", DateTime.Today.Month.ToString());
            parametro[6] = new ReportParameter("año", DateTime.Today.Year.ToString());

            string json ;
            lr.SetParameters(parametro);
            if (Session["user.tipo"].ToString() == "X" || Session["user.tipo"].ToString() == "D")
            {
                 json = JsonConvert.SerializeObject(db.lst_byAcred(0, actCursada.idActividadComplementaria, CalculaPeriodo()));
            }
            else
            {
                json = JsonConvert.SerializeObject(db.lst_byAcred(0, actCursada.idActividadComplementaria, periodo));
            }


            DataTable dtDetalleTF = JsonConvert.DeserializeObject<DataTable>(json);
            DataSet DSDetalleTF = new DataSet();
            DSDetalleTF.Tables.Add(dtDetalleTF);
            ReportDataSource rdtsDetalleTF = new ReportDataSource();
            rdtsDetalleTF.Name = "DTListas";
            rdtsDetalleTF.Value = DSDetalleTF.Tables[0];
            lr.DataSources.Add(rdtsDetalleTF);

            Response.ContentType = "application/force-download";
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = lr.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);

        }

        public ActionResult GenerarPdf(int id) // id = num_certificado_nac
        {
            ActividadCursada actCursada = db.ActividadCursada.Find(id);

            string idJefe = Session["user.id"].ToString();
            Maestros ma = db.Maestros.Find(idJefe);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Resources"), "Constancia.rdlc");
            lr.ReportPath = path;

            ReportParameter[] parametro = new ReportParameter[7];
            parametro[0] = new ReportParameter("departamento", actCursada.Grupos.ActividadComplementaria1.Departamento1.nombreDepartamento);
            parametro[1] = new ReportParameter("jd", ma.nombreMaestro);
            parametro[2] = new ReportParameter("creditos", actCursada.Grupos.ActividadComplementaria1.noCreditos.ToString());
            parametro[3] = new ReportParameter("ac", actCursada.Grupos.ActividadComplementaria1.nombreActComplementaria);
            parametro[4] = new ReportParameter("dia", DateTime.Today.Day.ToString());
            parametro[5] = new ReportParameter("mes", DateTime.Today.Month.ToString());
            parametro[6] = new ReportParameter("año", DateTime.Today.Year.ToString());


            lr.SetParameters(parametro);

            string json = JsonConvert.SerializeObject(db.lst_byAcred(actCursada.Estudiante.idEstudiante, actCursada.Grupos.ActividadComplementaria1.idActividadComplementaria, actCursada.periodo));


            DataTable dtDetalleTF = JsonConvert.DeserializeObject<DataTable>(json);
            DataSet DSDetalleTF = new DataSet();
            DSDetalleTF.Tables.Add(dtDetalleTF);
            ReportDataSource rdtsDetalleTF = new ReportDataSource();
            rdtsDetalleTF.Name = "DTListas";
            rdtsDetalleTF.Value = DSDetalleTF.Tables[0];
            lr.DataSources.Add(rdtsDetalleTF);

            Response.ContentType = "application/force-download";
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = lr.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);

        }

        public ActionResult Acuse(int id) // id = num_certificado_nac
        {
            ActividadCursada actCursada = db.ActividadCursada.Find(id);
            
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Resources"), "Acuse.rdlc");
            lr.ReportPath = path;

            ReportParameter[] parametro= new ReportParameter[5];
            parametro[0] = new ReportParameter("estudiante",actCursada.Estudiante.nombreEstudiante);
            parametro[1] = new ReportParameter("ctrlnum", actCursada.Estudiante.idEstudiante.ToString());
            parametro[2] = new ReportParameter("periodo", actCursada.periodo);
            parametro[3] = new ReportParameter("creditos", actCursada.Grupos.ActividadComplementaria1.noCreditos.ToString());
            parametro[4] = new ReportParameter("AC", actCursada.Grupos.ActividadComplementaria1.nombreActComplementaria);

            lr.SetParameters(parametro);


            

            Response.ContentType = "application/force-download";
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = lr.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);

        }

        public void Descagar(int idActa, MemoryStream ActComp)
        {
            const string nombre = "ActComplementarias.pdf";
            byte[] bytesInStream = ActComp.ToArray();
            ActComp.Close();

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment;    filename=" + nombre);
            Response.BinaryWrite(bytesInStream);
            Response.End();
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

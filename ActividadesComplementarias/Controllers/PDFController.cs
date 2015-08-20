using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using ActividadesComplementarias.Models;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;

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

        public void GenerarPdf(int id) // id = num_certificado_nac
        {
            ActividadCursada actCursada = db.ActividadCursada.Find(id);
            
            byte[] ActComplementarias = Recursos.Recursos.ActComplementariasPlantilla;
            MemoryStream ActComp = new MemoryStream();

            PdfReader pdfReader = new PdfReader(ActComplementarias);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, ActComp);
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            pdfFormFields.SetField("JefeDepartamento", "Joel Mendoza");
            pdfFormFields.SetField("Estudiante", actCursada.Estudiante.nombreEstudiante);
            pdfFormFields.SetField("NoControl", actCursada.idEstudiante.ToString()); 
            pdfFormFields.SetField("Carrera", actCursada.Estudiante.Carrera1.nombreCarrera);
            pdfFormFields.SetField("ActividadComplementaria", actCursada.ActividadComplementaria.nombreActComplementaria);
            pdfFormFields.SetField("Periodo", actCursada.periodo);
            pdfFormFields.SetField("Creditos", actCursada.ActividadComplementaria.noCreditos.ToString());
            pdfFormFields.SetField("Dia", DateTime.Today.Day.ToString());
            pdfFormFields.SetField("Mes", DateTime.Today.Month.ToString());
            pdfFormFields.SetField("Anio", DateTime.Today.Year.ToString());
            pdfFormFields.SetField("ProfesorFirma", "Luis Bravos");
            pdfFormFields.SetField("JefeDepartamentoFirma", "Joel Mendoza");
            pdfFormFields.SetField("Departamento", actCursada.Estudiante.Carrera1.Departamento1.nombreDepartamento);
            

            pdfStamper.FormFlattening = true;
            pdfStamper.Close();

            Descagar(id, ActComp);

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
            parametro[3] = new ReportParameter("creditos", actCursada.ActividadComplementaria.noCreditos.ToString());
            parametro[4] = new ReportParameter("AC", actCursada.ActividadComplementaria.nombreActComplementaria);

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

    }
}

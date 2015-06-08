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
            pdfFormFields.SetField("Departamento", "Sistemas e Informatica");
            

            pdfStamper.FormFlattening = true;
            pdfStamper.Close();

            Descagar(id, ActComp);

        }

        public void GenerarPdfAcuse(int id) // id = num_certificado_nac
        {
            ActividadCursada actCursada = db.ActividadCursada.Find(id);
            byte[] Acuse = Recursos.Recursos.AcuseActComp;
            MemoryStream AcuseComp = new MemoryStream();

            PdfReader pdfReader = new PdfReader(Acuse);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, AcuseComp);
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            pdfFormFields.SetField("ActividadComplementaria", actCursada.ActividadComplementaria.nombreActComplementaria);
            pdfFormFields.SetField("Estudiante", actCursada.Estudiante.nombreEstudiante);
            pdfFormFields.SetField("NoControl", actCursada.Estudiante.idEstudiante.ToString());
            
            pdfStamper.FormFlattening = true;
            pdfStamper.Close();

            DescagarAcuse(id, AcuseComp);

        }

        public void DescagarAcuse(int idActa, MemoryStream AcuseComp)
        {
            const string nombre = "AcuseActividadesComplementarias.pdf";
            byte[] bytesInStream = AcuseComp.ToArray();
            AcuseComp.Close();

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment;    filename=" + nombre);
            Response.BinaryWrite(bytesInStream);
            Response.End();
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

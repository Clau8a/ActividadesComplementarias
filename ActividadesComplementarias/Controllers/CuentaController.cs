using ActividadesComplementarias.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActividadesComplementarias.Controllers
{
    public class CuentaController : Controller
    {
        private CreditosComplementariosEntities db = new CreditosComplementariosEntities();
        //
        // GET: /Account/

        public ActionResult IniciarSesion()
        {
            return View();
        }

        ActividadesComplementarias.Models.Estudiante estudiante;
        ActividadesComplementarias.Models.Maestros maestro;
        // Post: /Account/
        [HttpPost]
        public RedirectResult IniciarSesion(string user, string password)
        {
            
            
            if ((user != "") && (password != ""))
            {
                if (findUser(user,password))
                {
                    Session["uxid"] = "lalala";
                }
            }
            return Redirect("/Home/Index");
        }

        private bool findUser(string usuario, string passwd) 
        {
            if (Char.IsNumber(usuario[0]))
            {//buscar en tabla de maestros

                estudiante = db.Estudiante.Find(Convert.ToInt64(usuario));
                if (estudiante != null)
                {
                    if (passwd == estudiante.contraseñaEstudiante)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {//buscar en tabla de estudiantes
                maestro = db.Maestros.Find(usuario);
                if (maestro != null)
                {
                    if (passwd == maestro.contraseñaMaestro)
                        return true;
                    else 
                        return false;
                }
                else
                    return false;    
            }
        }

    }
}

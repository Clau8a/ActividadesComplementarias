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
        char tipoUsuario;
        public ActionResult IniciarSesion()
        {
            return View();
        }

        ActividadesComplementarias.Models.Estudiante estudiante;
        ActividadesComplementarias.Models.Maestros maestro;
        // Post: /Account/
        [HttpPost]
        public ActionResult IniciarSesion(string user, string password)
        {
            if ((user != "") && (password != ""))
            {
                if (findUser(user, password))
                {
                    if (tipoUsuario == 'E')///Agregar el tipo de usuario
                        Session["uxid"] = estudiante.nombreEstudiante;
                    else
                        Session["uxid"] = maestro.nombreMaestro;

                }
                else 
                {
                    ViewBag.wrongUser = true;
                    return View(user, password);
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
                    {tipoUsuario='E';
                        return true;
                    }
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
                    {
                        tipoUsuario = Convert.ToChar(maestro.tipoMaestro);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;    
            }
        }

    }
}

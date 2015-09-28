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
    public class LoginController : Controller
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
                    Session["user.tipo"] = tipoUsuario;
                    if (tipoUsuario == 'E')
                    {///Agregar el tipo de usuario
                        Session["user"] = estudiante.nombreEstudiante;
                        Session["user.id"] = estudiante.idEstudiante;
                    }
                    else
                    {
                        Session["user"] = maestro.nombreMaestro;
                        Session["user.id"] = maestro.idMaestro;
                        if (tipoUsuario == 'C')
                            return Redirect("/Inscripcion");
                    }

                }
                else 
                {
                    ViewBag.wrongUser = true;
                    return View();
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
                        if (maestro.tipoMaestro != "N")
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
                else
                    return false;    
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return Redirect("/Home/Index");
        }
    }
}

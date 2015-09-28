using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActividadesComplementariasControllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            else
            {
                if (Session["user.tipo"].ToString() == "J")
                {
                    return RedirectToAction("Index", "Departamento");
                }
                else
                {
                    if (Session["user.tipo"].ToString() == "C")
                        return RedirectToAction("Index", "Inscripcion");
                    else
                    {
                        if (Session["user.tipo"].ToString() == "X" || Session["user.tipo"].ToString() == "D")
                        {
                            return RedirectToAction("Index", "ActividadComplementaria");
                        }
                        else
                        {
                            string id = Session["user.id"].ToString();
                            return Redirect("/ActividadCursada/Index/" + id);
                        }
                    }
                }

            }
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActividadesComplementarias.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["uxid"] == null)
            {
                return RedirectToAction("IniciarSesion", "Cuenta");
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
                        if (Session["user.tipo"].ToString() == "X")
                        {
                            return RedirectToAction("Index", "ActividadComplementaria");
                        }
                        else
                        {
                            string id = Session["user.id"].ToString();
                            return Redirect("/Inscripcion/Index/" + id);
                        }
                    }
                }

            }
        }

    }
}

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
            return View();
        }

    }
}

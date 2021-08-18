using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBelediyeOtomasyonu.Controllers;

namespace MvcBelediyeOtomasyonu.Controllers
{
    //[Authorize]
    [Authorize(Roles = "A,U,MA,MU,RA,RU")]
    public class HomeController : BaseController
    {
        // GET: Home
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            SiteBilgiGetir();
            return View();
        }
    }
}
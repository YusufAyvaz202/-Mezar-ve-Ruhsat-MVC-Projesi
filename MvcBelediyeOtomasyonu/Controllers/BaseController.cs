using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcBelediyeOtomasyonu.Controllers
{
    //[Authorize]
    [Authorize(Roles = "A,U,MA,MU,RA,RU")]
    public class BaseController : Controller
    {
        public int degisken = -1;
        // GET: Base
        
        public Entities db = new Entities();

        public void SiteBilgiGetir()
        {
            string bugun = DateTime.Now.ToShortDateString();

            SecurityController sec = new SecurityController();
            //HttpContext.Current.User.Identity.Name.Split('|')[0]
            // HttpContext cont = new HttpContext();
            //HttpCookie cookie = FormsAuthentication.GetAuthCookie("userName", false);
            //int userId = Convert.ToInt32(cookie.Name.Split('|')[1]);
            //HttpContext.Current.User.Identity.Name.Split('|')[0]
            //string userNameCookie = HttpContext.Request.Cookies["userName"].Value.ToString();

            int UserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
            ViewData["RuhsatSayisi"] = db.Ruhsatlar.Count();
            ViewData["KullaniciSayisi"] = db.Kullanicilar.Count();
            ViewData["RuhsatTipSayisi"] = db.RuhsatTipleri.Count();
            ViewData["KullaniciTipSayisi"] = db.KullaniciTipleri.Count();
            ViewData["MezarlikSayisi"] = db.Mezarliklar.Count();
            ViewData["AdaSayisi"] = db.Adalar.Count();
            ViewData["MezarSayisi"] = db.MezarYerleri.Count();
            ViewData["KirsalRuhsatSayisi"] = db.KirsalYapiRuhsatlari.Count();

            /*Bugün kaydedilen kayıt sayıları*/
            ViewData["SonRuhsatSayisi"] = db.Ruhsatlar.Where(x => x.SaveDate.Substring(0,10) == bugun.ToString()).Count();
            ViewData["SonKirsalRuhsatSayisi"] = db.KirsalYapiRuhsatlari.Where(x => x.SaveDate.Substring(0, 10) == bugun.ToString()).Count();
            ViewData["SonMezarSayisi"] = db.MezarYerleri.Where(x => x.SaveDate.Substring(0, 10) == bugun.ToString()).Count();

            /*Kaydedilme sayıları*/
            /*Id değerleri değiştirilecek*/
            ViewData["KayitRuhsatSayisi"] = db.Ruhsatlar.Where(x => x.SaveUserId == UserId).Count();
            ViewData["KayitRuhsatSayisiYuzde"] = Convert.ToInt32(ViewData["KayitRuhsatSayisi"]) * 100 / Convert.ToInt32(ViewData["RuhsatSayisi"]);

            ViewData["KayitKirsalRuhsatSayisi"] = db.KirsalYapiRuhsatlari.Where(x => x.SaveUserId == UserId).Count();
            ViewData["KayitKirsalRuhsatSayisiYuzde"] = Convert.ToInt32(ViewData["KayitKirsalRuhsatSayisi"]) * 100 / Convert.ToInt32(ViewData["KirsalRuhsatSayisi"]);

            ViewData["KayitMezarSayisi"] = db.MezarYerleri.Where(x => x.SaveUserId == UserId).Count();
            ViewData["KayitMezarSayisiYuzde"] = Convert.ToInt32(ViewData["KayitMezarSayisi"]) * 100 / Convert.ToInt32(ViewData["MezarSayisi"]);

            /*Değiştirilme sayıları*/
            /*Id değerleri değiştirilecek*/
            ViewData["EditRuhsatSayisi"] = db.Ruhsatlar.Where(x => x.EditUserId == UserId).Count();
            ViewData["EditRuhsatSayisiYuzde"] = Convert.ToInt32(ViewData["EditRuhsatSayisi"]) * 100 / Convert.ToInt32(ViewData["RuhsatSayisi"]);

            ViewData["EditKirsalRuhsatSayisi"] = db.KirsalYapiRuhsatlari.Where(x => x.EditUserId == UserId).Count();
            ViewData["EditKirsalRuhsatSayisiYuzde"] = Convert.ToInt32(ViewData["EditKirsalRuhsatSayisi"]) * 100 / Convert.ToInt32(ViewData["KirsalRuhsatSayisi"]);

            ViewData["EditMezarSayisi"] = db.MezarYerleri.Where(x => x.EditUserId == UserId).Count();
            ViewData["EditMezarSayisiYuzde"] = Convert.ToInt32(ViewData["EditMezarSayisi"]) * 100 / Convert.ToInt32(ViewData["MezarSayisi"]);


            //ViewData["KullaniciTipi"] = db.KullaniciTipleri.Where(x=>x.Id == sec)

        }

        protected override void Dispose(bool disposing)
        {
            
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
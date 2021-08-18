using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcBelediyeOtomasyonu.Controllers
{
    [Authorize(Roles = "SA,A,SU,U")]
    public class SecurityController : BaseController
    {
        public int UserId = -1;
        // GET: Security
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Kullanicilar kullanici)
        {
            var kullaniciInDb = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullanici.KullaniciAdi && x.Sifre == kullanici.Sifre);
            if (kullaniciInDb != null)
            {

                FormsAuthentication.SetAuthCookie(kullaniciInDb.Ad + " " + kullaniciInDb.Soyad + "|" + kullaniciInDb.Id + "|" + kullaniciInDb.KullaniciTipId + "|" + kullaniciInDb.ImagePath + "|" + kullaniciInDb.Unvan , false);
                //HttpCookie cookie = new HttpCookie("cook", kullaniciInDb.Id.ToString());
                //GetUserId(kullaniciInDb.Id);
                //TempData["UserId"] = kullaniciInDb.Id;
                //int tipId = Convert.ToInt32(kullaniciInDb.KullaniciTipId);
                //ViewData["KullaniciTipi"] = 1;
                //Bilgi(kullaniciInDb.Id);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mesaj = "Hata : Geçersiz Kullanıcı Adı veya Şifre";
                return View();
            }

        }

        //public void Bilgi(int id)
        //{
        //    ViewData["RuhsatSayisi"] = db.Ruhsatlar.Count();
        //    ViewData["KayitRuhsatSayisi"] = db.Ruhsatlar.Where(x => x.SaveUserId == id).Count();
        //    ViewData["KayitRuhsatSayisiYuzde"] = Convert.ToInt32(ViewData["KayitRuhsatSayisi"]) * 100 / Convert.ToInt32(ViewData["RuhsatSayisi"]);
        //}



        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
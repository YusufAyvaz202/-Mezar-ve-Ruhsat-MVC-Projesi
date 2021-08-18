using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBelediyeOtomasyonu.Controllers
{
    [Authorize(Roles = "A")]
    public class KullaniciTipController : BaseController
    {
        // GET: KullaniciTip
        public ActionResult Index()
        {
            SiteBilgiGetir();
            return View(db.KullaniciTipleri.ToList());
        }

        public ActionResult Yeni()
        {
            SiteBilgiGetir();
            var model = new KullaniciTipleri();
            return View("KullaniciTipForm", model);
        }


        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(KullaniciTipleri p)
        {
            if (!ModelState.IsValid)
            {
                return View("Yeni");
            }


            if (p.Id == 0) //YENİ KAYIT İŞLEMİ
            {
                p.SaveDate = DateTime.Now.ToString();
                p.SaveUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                db.Entry(p).State = System.Data.Entity.EntityState.Added;
            }
            else //GÜNCELLEME İŞLEMİ
            {
                p.EditDate = DateTime.Now.ToString();
                p.EditUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult BilgiGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.KullaniciTipleri.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("KullaniciTipForm", model);
        }

        public ActionResult Sil(KullaniciTipleri p)
        {
            var KullaniciTiplerinDb = db.Kullanicilar.FirstOrDefault(x => x.KullaniciTipId == p.Id);

            if (KullaniciTiplerinDb == null)
            {
                db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mesaj = "Hata : Seçilen kullanıcı tipine bağlı kayıtlar var...";
                return View("Index", db.KullaniciTipleri.ToList());
            }
        }

        public ActionResult DetayGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.KullaniciTipleri.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("KullaniciTipDetay", model);
        }
    }
}
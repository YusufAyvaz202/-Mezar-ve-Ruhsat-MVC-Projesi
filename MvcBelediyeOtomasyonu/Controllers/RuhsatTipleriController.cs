using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBelediyeOtomasyonu.Controllers
{
    
    public class RuhsatTipleriController : BaseController
    {
        // GET: RuhsatTipleri
        [Authorize(Roles = "A,RA,U,RU")]
        public ActionResult Index()
        {
            SiteBilgiGetir();
            return View(db.RuhsatTipleri.ToList());
        }
        [Authorize(Roles = "A,RA,U,RU")]
        public ActionResult Yeni()
        {
            SiteBilgiGetir();
            var model = new RuhsatTipleri();            
            return View("RuhsatTipForm", model);
        }


        [ValidateAntiForgeryToken]
        [Authorize(Roles = "A,RA,U")]
        public ActionResult Kaydet(RuhsatTipleri p)
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

        [Authorize(Roles = "A,RA,U")]
        public ActionResult BilgiGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.RuhsatTipleri.Find(id);
           
            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("RuhsatTipForm", model);
        }

        [Authorize(Roles = "A,RA,U")]
        public ActionResult Sil(RuhsatTipleri p)
        {
            var RuhsatTiplerinDb = db.Ruhsatlar.FirstOrDefault(x => x.RuhsatTipId == p.Id);

            if (RuhsatTiplerinDb == null)
            {
                db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mesaj = "Hata : Seçilen ruhsat tipine bağlı kayıtlar var...";
                return View("Index", db.RuhsatTipleri.ToList());
            }
        }

        [Authorize(Roles = "A,RA,U,RU")]
        public ActionResult DetayGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.RuhsatTipleri.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("RuhsatTipleriDetay", model);
        }
    }
}
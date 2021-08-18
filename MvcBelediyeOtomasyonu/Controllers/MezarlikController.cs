using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBelediyeOtomasyonu.Controllers
{
    
    public class MezarlikController : BaseController
    {
        // GET: Mezarlik
        [Authorize(Roles = "A,U,MA,MU")]
        public ActionResult Index()
        {
            SiteBilgiGetir();
            return View(db.Mezarliklar.ToList());
        }
        [Authorize(Roles = "A,MA,U")]
        public ActionResult Yeni()
        {
            SiteBilgiGetir();
            var model = new Mezarliklar();
            return View("MezarlikForm", model);
        }


        [ValidateAntiForgeryToken]
        [Authorize(Roles = "A,MA,U")]
        public ActionResult Kaydet(Mezarliklar p)
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

        [Authorize(Roles = "A,MA,U")]
        public ActionResult BilgiGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.Mezarliklar.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("MezarlikForm", model);
        }

        [Authorize(Roles = "A,MA,U")]
        public ActionResult Sil(Mezarliklar p)
        {
            var mezarlikinDb = db.Adalar.FirstOrDefault(x => x.MezarlikId == p.Id);

            if (mezarlikinDb == null)
            {
                db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mesaj = "Hata : Seçilen mezarlık içerisinde kayıtlı adalar var...";
                return View("Index", db.Mezarliklar.ToList());
            }
        }

        [Authorize(Roles = "A,MA,U,MU")]
        public ActionResult DetayGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.Mezarliklar.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("MezarlikDetay", model);
        }
    }
}
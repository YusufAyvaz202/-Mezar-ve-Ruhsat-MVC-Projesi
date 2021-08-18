using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBelediyeOtomasyonu.Controllers
{
    [Authorize(Roles = "A")]
    public class UsersController : BaseController
    {
        // GET: Users
        public ActionResult Index()
        {
            SiteBilgiGetir();
            return View(db.Kullanicilar.ToList());
        }

        public ActionResult Yeni()
        {
            SiteBilgiGetir();
            var model = new Kullanicilar();
            DropDownDoldur(model);
            return View("UserForm", model);
        }


        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(Kullanicilar p, HttpPostedFileBase ImagePath)
        {
            if (!ModelState.IsValid)
            {
                return View("Yeni");
            }
            string sonImagePath = p.ImagePath;
            string sonSaveUserId = Convert.ToInt32(p.SaveUserId).ToString();
            Random rastgele = new Random();
            int sayi = rastgele.Next(1000, 10000);

            if (p.Id == 0) //YENİ KAYIT İŞLEMİ
            {

                if (ImagePath != null)
                {
                    string dosyaadi = sayi + Path.GetFileName(ImagePath.FileName);
                    string uzanti = Path.GetExtension(ImagePath.FileName);
                    string yol = "~/images/" + dosyaadi;
                    ImagePath.SaveAs(Server.MapPath(yol));
                    p.ImagePath = "images/" + dosyaadi;
                }
                p.Silindi = false;
                p.SaveDate = DateTime.Now.ToString();
                p.SaveUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                db.Entry(p).State = System.Data.Entity.EntityState.Added;
            }
            else //GÜNCELLEME İŞLEMİ
            {
                if (ImagePath != null)
                {
                    string dosyaadi = sayi + Path.GetFileName(ImagePath.FileName);
                    string uzanti = Path.GetExtension(ImagePath.FileName);
                    string yol = "~/images/" + dosyaadi;
                    ImagePath.SaveAs(Server.MapPath(yol));
                    p.ImagePath = "images/" + dosyaadi;
                }
                else
                {
                    p.ImagePath = sonImagePath;
                }
                p.Silindi = false;
                p.SaveUserId = Convert.ToInt32(sonSaveUserId);
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
            var model = db.Kullanicilar.Find(id);
            DropDownDoldur(model);
            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("UserForm", model);
        }

        public ActionResult Sil(Kullanicilar p)
        {
            var update = db.Kullanicilar.Find(p.Id);
            update.Aktif = false;
            db.Entry(update).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DetayGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.Kullanicilar.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("UserDetay", model);
        }

        private void DropDownDoldur(Kullanicilar model)
        {
            List<KullaniciTipleri> kullaniciTipList = db.KullaniciTipleri.OrderBy(x => x.Tip).ToList();
            model.KullaniciTipListesi = (from x in kullaniciTipList
                                         select new SelectListItem
                                         {
                                             Text = x.Tip,
                                             Value = x.Id.ToString()
                                         }).ToList();

        }

        public ActionResult ResimSil(string File, Kullanicilar p)
        {
            if (File == "kullaniciresim")
            {
                var silinecek = db.Kullanicilar.Find(p.Id);
                if (silinecek.ImagePath != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.ImagePath));
                    silinecek.ImagePath = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult FormResimSil(string File, Kullanicilar p)
        {
            if (File == "kullaniciresim")
            {
                var silinecek = db.Kullanicilar.Find(p.Id);
                if (silinecek.ImagePath != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.ImagePath));
                    silinecek.ImagePath = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("BilgiGetir", new { id = p.Id });
        }
    }
}
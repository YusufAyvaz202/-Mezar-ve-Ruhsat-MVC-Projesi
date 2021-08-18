using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;

namespace MvcBelediyeOtomasyonu.Controllers
{
    [Authorize(Roles = "A,U,RA,RU,MA,MU")]
    public class ProfileController : BaseController
    {
        // GET: Profile
        

        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(Kullanicilar p, HttpPostedFileBase ImagePath)
        {
            if (!ModelState.IsValid)
            {
                return View("Yeni");
            }

            string sonImagePath = p.ImagePath;
            Random rastgele = new Random();
            int sayi = rastgele.Next(1000, 10000);

            if (p.Id != 0) //GÜNCELLEME İŞLEMİ
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
                p.EditDate = DateTime.Now.ToString();
                p.EditUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();


            return RedirectToAction("Index", "Home");
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
            return View("ProfileForm", model);
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
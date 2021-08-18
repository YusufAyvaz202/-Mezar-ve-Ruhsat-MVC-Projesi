using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBelediyeOtomasyonu.Controllers
{
    
    public class KirsalRuhsatController : BaseController
    {
        // GET: KirsalRuhsat
        [Authorize(Roles = "A,RA,U,RU")]
        public ActionResult Index(int sayfa = 1)
        {
            SiteBilgiGetir();
            var degerler = db.KirsalYapiRuhsatlari.ToList().ToPagedList(sayfa, 20);
            return View(degerler);
        }

        [Authorize(Roles = "A,RA,U,RU")]
        public ActionResult Search(int search, string p)
        {
            SiteBilgiGetir();
            var degerler = from d in db.KirsalYapiRuhsatlari select d;
            if (!string.IsNullOrEmpty(p))
            {
                if (search == 1)
                {
                    //listBox1.Items.Clear();
                    int sayac = 1;
                    string adaNo = "", parselNo = "";
                    char[] karakterler = p.ToCharArray();
                    foreach (char karakter in karakterler)
                    {
                        if (karakter.ToString() != " ")
                        {
                            adaNo += karakter.ToString();
                            sayac++;
                        }
                        else
                        {
                            parselNo = p.Substring(sayac);
                            break;
                        }
                    }
                    //(x => x.MezarlikId == p.MezarlikId)
                    degerler = degerler.Where(m => m.Ada.ToString().Contains(adaNo));
                    degerler = degerler.Where(m => m.Parsel.ToString().Contains(parselNo));
                    //degerler = degerler1;
                }
                else if (search == 2)
                {
                    degerler = degerler.Where(m => m.YapiSahibi.Contains(p));
                }
                else if (search == 3)
                {
                    degerler = degerler.Where(m => m.TcNo.Contains(p));
                }
            }
            return View(degerler.ToList());
        }

        [Authorize(Roles = "A,RA,U")]
        public ActionResult Yeni()
        {
            SiteBilgiGetir();
            var model = new KirsalYapiRuhsatlari();
            DropDownDoldur(model);
            return View("KirsalRuhsatForm", model);
        }


        [ValidateAntiForgeryToken]
        [Authorize(Roles = "A,RA,U")]
        public ActionResult Kaydet(KirsalYapiRuhsatlari p, HttpPostedFileBase Tarama0, HttpPostedFileBase Tarama1, HttpPostedFileBase Tarama2, HttpPostedFileBase ProjeDosyasi)
        {
            if (!ModelState.IsValid)
            {
                return View("Yeni", new KirsalYapiRuhsatlari());
            }

            string sonTarama0 = p.Tarama0;
            string sonTarama1 = p.Tarama1;
            string sonTarama2 = p.Tarama2;
            string sonProjeDosyasi = p.ProjeDosyasi;
            Random rastgele = new Random();
            int sayi0 = rastgele.Next(1000, 10000);
            int sayi1 = rastgele.Next(1000, 10000);
            int sayi2 = rastgele.Next(1000, 10000);
            int sayi10 = rastgele.Next(1000, 10000);


            if (p.Id == 0) //YENİ KAYIT İŞLEMİ
            {
                if (Tarama0 != null)
                {
                    string dosyaadi = sayi0 + Path.GetFileName(Tarama0.FileName);
                    string uzanti = Path.GetExtension(Tarama0.FileName);
                    string yol = "~/files/kirsalruhsatlar/" + dosyaadi;
                    Tarama0.SaveAs(Server.MapPath(yol));
                    p.Tarama0 = "/files/kirsalruhsatlar/" + dosyaadi;
                }
                if (Tarama1 != null)
                {
                    string dosyaadi = sayi1 + Path.GetFileName(Tarama1.FileName);
                    string uzanti = Path.GetExtension(Tarama1.FileName);
                    string yol = "~/files/kirsalruhsatlar/" + dosyaadi;
                    Tarama1.SaveAs(Server.MapPath(yol));
                    p.Tarama1 = "/files/kirsalruhsatlar/" + dosyaadi;
                }
                if (Tarama2 != null)
                {
                    string dosyaadi = sayi2 + Path.GetFileName(Tarama2.FileName);
                    string uzanti = Path.GetExtension(Tarama2.FileName);
                    string yol = "~/files/kirsalruhsatlar/" + dosyaadi;
                    Tarama2.SaveAs(Server.MapPath(yol));
                    p.Tarama2 = "/files/kirsalruhsatlar/" + dosyaadi;
                }
                if (ProjeDosyasi != null)
                {
                    string dosyaadi = sayi10 + Path.GetFileName(ProjeDosyasi.FileName);
                    string uzanti = Path.GetExtension(ProjeDosyasi.FileName);
                    string yol = "~/files/kirsalproje/" + dosyaadi;
                    ProjeDosyasi.SaveAs(Server.MapPath(yol));
                    p.ProjeDosyasi = "/files/kirsalproje/" + dosyaadi;
                }
                p.SaveDate = DateTime.Now.ToString();
                p.SaveUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                //p.Silindi = false;
                db.Entry(p).State = System.Data.Entity.EntityState.Added;
            }
            else //GÜNCELLEME İŞLEMİ
            {
                if (Tarama0 != null)
                {
                    string dosyaadi = sayi0 + Path.GetFileName(Tarama0.FileName);
                    string uzanti = Path.GetExtension(Tarama0.FileName);
                    string yol = "~/files/kirsalruhsatlar/" + dosyaadi;
                    Tarama0.SaveAs(Server.MapPath(yol));
                    p.Tarama0 = "/files/kirsalruhsatlar/" + dosyaadi;
                }
                else
                {
                    p.Tarama0 = sonTarama0;
                }
                if (Tarama1 != null)
                {
                    string dosyaadi = sayi1 + Path.GetFileName(Tarama1.FileName);
                    string uzanti = Path.GetExtension(Tarama1.FileName);
                    string yol = "~/files/kirsalruhsatlar/" + dosyaadi;
                    Tarama1.SaveAs(Server.MapPath(yol));
                    p.Tarama1 = "/files/kirsalruhsatlar/" + dosyaadi;
                }
                else
                {
                    p.Tarama1 = sonTarama1;
                }
                if (Tarama2 != null)
                {
                    string dosyaadi = sayi2 + Path.GetFileName(Tarama2.FileName);
                    string uzanti = Path.GetExtension(Tarama2.FileName);
                    string yol = "~/files/kirsalruhsatlar/" + dosyaadi;
                    Tarama2.SaveAs(Server.MapPath(yol));
                    p.Tarama2 = "/files/kirsalruhsatlar/" + dosyaadi;
                }
                else
                {
                    p.Tarama2 = sonTarama2;
                }
                if (ProjeDosyasi != null)
                {
                    string dosyaadi = sayi10 + Path.GetFileName(ProjeDosyasi.FileName);
                    string uzanti = Path.GetExtension(ProjeDosyasi.FileName);
                    string yol = "~/files/kirsalproje/" + dosyaadi;
                    ProjeDosyasi.SaveAs(Server.MapPath(yol));
                    p.ProjeDosyasi = "/files/kirsalproje/" + dosyaadi;
                }
                else
                {
                    p.ProjeDosyasi = sonProjeDosyasi;
                }

                p.EditDate = DateTime.Now.ToString();
                p.EditUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                //p.EditDate = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();


            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A,RA,U")]
        public ActionResult BilgiGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.KirsalYapiRuhsatlari.Find(id);
            DropDownDoldur(model);
            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("KirsalRuhsatForm", model);
        }

        [Authorize(Roles = "A,RA,U")]
        public ActionResult Sil(KirsalYapiRuhsatlari p)
        {
            var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
            if (silinecek.Tarama0 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama0));
            }
            if (silinecek.Tarama1 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama1));
            }
            if (silinecek.Tarama2 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama2));
            }
            if (silinecek.ProjeDosyasi != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.ProjeDosyasi));
            }

            db.Entry(silinecek).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A,RA,U,RU")]
        public ActionResult DetayGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.KirsalYapiRuhsatlari.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("KirsalRuhsatDetay", model);
        }

        private void DropDownDoldur(KirsalYapiRuhsatlari model)
        {
            List<RuhsatTipleri> kirsalRuhsatTipList = db.RuhsatTipleri.OrderBy(x => x.Tip).ToList();
            model.RuhsatTipListesi = (from x in kirsalRuhsatTipList
                                      select new SelectListItem
                                      {
                                          Text = x.Tip,
                                          Value = x.Id.ToString()
                                      }).ToList();


        }

        [Authorize(Roles = "A,RA,U")]
        public ActionResult DosyaSil(string File, KirsalYapiRuhsatlari p)
        {

            if (File == "file0")
            {
                var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
                if (silinecek.Tarama0 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama0));
                    silinecek.Tarama0 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file1")
            {
                var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
                if (silinecek.Tarama1 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama1));
                    silinecek.Tarama1 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file2")
            {
                var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
                if (silinecek.Tarama2 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama2));
                    silinecek.Tarama2 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "proje")
            {
                var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
                if (silinecek.ProjeDosyasi != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.ProjeDosyasi));
                    silinecek.ProjeDosyasi = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("DetayGetir", new { id = p.Id });
        }

        [Authorize(Roles = "A,RA,U")]
        public ActionResult TaramaSil(string File, KirsalYapiRuhsatlari p)
        {

            if (File == "file0")
            {
                var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
                if (silinecek.Tarama0 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama0));
                    silinecek.Tarama0 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file1")
            {
                var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
                if (silinecek.Tarama1 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama1));
                    silinecek.Tarama1 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file2")
            {
                var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
                if (silinecek.Tarama2 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama2));
                    silinecek.Tarama2 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "proje")
            {
                var silinecek = db.KirsalYapiRuhsatlari.Find(p.Id);
                if (silinecek.ProjeDosyasi != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.ProjeDosyasi));
                    silinecek.ProjeDosyasi = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("BilgiGetir", new { id = p.Id });
        }

    }
}
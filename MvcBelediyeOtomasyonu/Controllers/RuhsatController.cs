using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBelediyeOtomasyonu.Controllers;
using System.IO;
using PagedList;
using PagedList.Mvc;

namespace MvcBelediyeOtomasyonu.Controllers
{
    //[Authorize]
    
    public class RuhsatController : BaseController
    {
        // GET: Ruhsat
        [Authorize(Roles = "A,RA,U,RU")]
        public ActionResult Index(int sayfa = 1)
        {
            SiteBilgiGetir();
            var degerler = db.Ruhsatlar.ToList().ToPagedList(sayfa, 20);
            return View(degerler);
        }
        [Authorize(Roles = "A,RA,U,RU")]
        public ActionResult Search(int search, string p)
        {
            SiteBilgiGetir();
            var degerler = from d in db.Ruhsatlar select d;
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
                    degerler = degerler.Where(m => m.Ada.Contains(adaNo));
                    degerler = degerler.Where(m => m.Parsel.Contains(parselNo));
                    //degerler = degerler1;
                }
                else if (search == 2)
                {
                    degerler = degerler.Where(m => m.RuhsatSahibi.Contains(p));
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
            var model = new Ruhsatlar();
            DropDownDoldur(model);
            return View("RuhsatForm", model);
        }


        [ValidateAntiForgeryToken]
        [Authorize(Roles = "A,RA,U")]
        public ActionResult Kaydet(Ruhsatlar p, HttpPostedFileBase Tarama0, HttpPostedFileBase Tarama1, HttpPostedFileBase Tarama2, HttpPostedFileBase Tarama3, HttpPostedFileBase Tarama4, HttpPostedFileBase Tarama5, HttpPostedFileBase Tarama6, HttpPostedFileBase Tarama7, HttpPostedFileBase Tarama8, HttpPostedFileBase Tarama9, HttpPostedFileBase ProjeDosyasi)
        {
            if (!ModelState.IsValid)
            {
                return View("Yeni", new Ruhsatlar());
            }

            string sonTarama0 = p.Tarama0;
            string sonTarama1 = p.Tarama1;
            string sonTarama2 = p.Tarama2;
            string sonTarama3 = p.Tarama3;
            string sonTarama4 = p.Tarama4;
            string sonTarama5 = p.Tarama5;
            string sonTarama6 = p.Tarama6;
            string sonTarama7 = p.Tarama7;
            string sonTarama8 = p.Tarama8;
            string sonTarama9 = p.Tarama9;
            string sonProjeDosyasi = p.ProjeDosyasi;
            Random rastgele = new Random();
            int sayi0 = rastgele.Next(1000, 10000);
            int sayi1 = rastgele.Next(1000, 10000);
            int sayi2 = rastgele.Next(1000, 10000);
            int sayi3 = rastgele.Next(1000, 10000);
            int sayi4 = rastgele.Next(1000, 10000);
            int sayi5 = rastgele.Next(1000, 10000);
            int sayi6 = rastgele.Next(1000, 10000);
            int sayi7 = rastgele.Next(1000, 10000);
            int sayi8 = rastgele.Next(1000, 10000);
            int sayi9 = rastgele.Next(1000, 10000);
            int sayi10 = rastgele.Next(1000, 10000);


            if (p.Id == 0) //YENİ KAYIT İŞLEMİ
            {
                if (Tarama0 != null)
                {
                    string dosyaadi = sayi0 + Path.GetFileName(Tarama0.FileName);
                    string uzanti = Path.GetExtension(Tarama0.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama0.SaveAs(Server.MapPath(yol));
                    p.Tarama0 = "/files/" + dosyaadi;
                }
                if (Tarama1 != null)
                {
                    string dosyaadi = sayi1 + Path.GetFileName(Tarama1.FileName);
                    string uzanti = Path.GetExtension(Tarama1.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama1.SaveAs(Server.MapPath(yol));
                    p.Tarama1 = "/files/" + dosyaadi;
                }
                if (Tarama2 != null)
                {
                    string dosyaadi = sayi2 + Path.GetFileName(Tarama2.FileName);
                    string uzanti = Path.GetExtension(Tarama2.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama2.SaveAs(Server.MapPath(yol));
                    p.Tarama2 = "/files/" + dosyaadi;
                }
                if (Tarama3 != null)
                {
                    string dosyaadi = sayi3 + Path.GetFileName(Tarama3.FileName);
                    string uzanti = Path.GetExtension(Tarama3.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama3.SaveAs(Server.MapPath(yol));
                    p.Tarama3 = "/files/" + dosyaadi;
                }
                if (Tarama4 != null)
                {
                    string dosyaadi = sayi4 + Path.GetFileName(Tarama4.FileName);
                    string uzanti = Path.GetExtension(Tarama4.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama4.SaveAs(Server.MapPath(yol));
                    p.Tarama4 = "/files/" + dosyaadi;
                }
                if (Tarama5 != null)
                {
                    string dosyaadi = sayi5 + Path.GetFileName(Tarama5.FileName);
                    string uzanti = Path.GetExtension(Tarama5.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama5.SaveAs(Server.MapPath(yol));
                    p.Tarama5 = "/files/" + dosyaadi;
                }
                if (Tarama6 != null)
                {
                    string dosyaadi = sayi6 + Path.GetFileName(Tarama6.FileName);
                    string uzanti = Path.GetExtension(Tarama6.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama6.SaveAs(Server.MapPath(yol));
                    p.Tarama6 = "/files/" + dosyaadi;
                }
                if (Tarama7 != null)
                {
                    string dosyaadi = sayi7 + Path.GetFileName(Tarama7.FileName);
                    string uzanti = Path.GetExtension(Tarama7.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama7.SaveAs(Server.MapPath(yol));
                    p.Tarama7 = "/files/" + dosyaadi;
                }
                if (Tarama8 != null)
                {
                    string dosyaadi = sayi8 + Path.GetFileName(Tarama8.FileName);
                    string uzanti = Path.GetExtension(Tarama8.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama8.SaveAs(Server.MapPath(yol));
                    p.Tarama8 = "/files/" + dosyaadi;
                }
                if (Tarama9 != null)
                {
                    string dosyaadi = sayi9 + Path.GetFileName(Tarama9.FileName);
                    string uzanti = Path.GetExtension(Tarama9.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama9.SaveAs(Server.MapPath(yol));
                    p.Tarama9 = "/files/" + dosyaadi;
                }
                if (ProjeDosyasi != null)
                {
                    string dosyaadi = sayi10 + Path.GetFileName(ProjeDosyasi.FileName);
                    string uzanti = Path.GetExtension(ProjeDosyasi.FileName);
                    string yol = "~/files/proje/" + dosyaadi;
                    ProjeDosyasi.SaveAs(Server.MapPath(yol));
                    p.ProjeDosyasi = "/files/proje/" + dosyaadi;
                }
                p.SaveDate = DateTime.Now.ToString();
                p.SaveUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                p.Silindi = false;
                db.Entry(p).State = System.Data.Entity.EntityState.Added;
            }
            else //GÜNCELLEME İŞLEMİ
            {
                if (Tarama0 != null)
                {
                    string dosyaadi = sayi0 + Path.GetFileName(Tarama0.FileName);
                    string uzanti = Path.GetExtension(Tarama0.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama0.SaveAs(Server.MapPath(yol));
                    p.Tarama0 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama0 = sonTarama0;
                }
                if (Tarama1 != null)
                {
                    string dosyaadi = sayi1 + Path.GetFileName(Tarama1.FileName);
                    string uzanti = Path.GetExtension(Tarama1.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama1.SaveAs(Server.MapPath(yol));
                    p.Tarama1 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama1 = sonTarama1;
                }
                if (Tarama2 != null)
                {
                    string dosyaadi = sayi2 + Path.GetFileName(Tarama2.FileName);
                    string uzanti = Path.GetExtension(Tarama2.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama2.SaveAs(Server.MapPath(yol));
                    p.Tarama2 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama2 = sonTarama2;
                }
                if (Tarama3 != null)
                {
                    string dosyaadi = sayi3 + Path.GetFileName(Tarama3.FileName);
                    string uzanti = Path.GetExtension(Tarama3.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama3.SaveAs(Server.MapPath(yol));
                    p.Tarama3 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama3 = sonTarama3;
                }
                if (Tarama4 != null)
                {
                    string dosyaadi = sayi4 + Path.GetFileName(Tarama4.FileName);
                    string uzanti = Path.GetExtension(Tarama4.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama4.SaveAs(Server.MapPath(yol));
                    p.Tarama4 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama4 = sonTarama4;
                }
                if (Tarama5 != null)
                {
                    string dosyaadi = sayi5 + Path.GetFileName(Tarama5.FileName);
                    string uzanti = Path.GetExtension(Tarama5.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama5.SaveAs(Server.MapPath(yol));
                    p.Tarama5 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama5 = sonTarama5;
                }
                if (Tarama6 != null)
                {
                    string dosyaadi = sayi6 + Path.GetFileName(Tarama6.FileName);
                    string uzanti = Path.GetExtension(Tarama6.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama6.SaveAs(Server.MapPath(yol));
                    p.Tarama6 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama6 = sonTarama6;
                }
                if (Tarama7 != null)
                {
                    string dosyaadi = sayi7 + Path.GetFileName(Tarama7.FileName);
                    string uzanti = Path.GetExtension(Tarama7.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama7.SaveAs(Server.MapPath(yol));
                    p.Tarama7 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama7 = sonTarama7;
                }
                if (Tarama8 != null)
                {
                    string dosyaadi = sayi8 + Path.GetFileName(Tarama8.FileName);
                    string uzanti = Path.GetExtension(Tarama8.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama8.SaveAs(Server.MapPath(yol));
                    p.Tarama8 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama8 = sonTarama8;
                }
                if (Tarama9 != null)
                {
                    string dosyaadi = sayi9 + Path.GetFileName(Tarama9.FileName);
                    string uzanti = Path.GetExtension(Tarama9.FileName);
                    string yol = "~/files/" + dosyaadi;
                    Tarama9.SaveAs(Server.MapPath(yol));
                    p.Tarama9 = "/files/" + dosyaadi;
                }
                else
                {
                    p.Tarama9 = sonTarama9;
                }
                if (ProjeDosyasi != null)
                {
                    string dosyaadi = sayi10 + Path.GetFileName(ProjeDosyasi.FileName);
                    string uzanti = Path.GetExtension(ProjeDosyasi.FileName);
                    string yol = "~/files/proje/" + dosyaadi;
                    ProjeDosyasi.SaveAs(Server.MapPath(yol));
                    p.ProjeDosyasi = "/files/proje/" + dosyaadi;
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
            var model = db.Ruhsatlar.Find(id);
            DropDownDoldur(model);
            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("RuhsatForm", model);
        }

        [Authorize(Roles = "A,RA,U")]
        public ActionResult Sil(Ruhsatlar p)
        {
            var silinecek = db.Ruhsatlar.Find(p.Id);
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
            if (silinecek.Tarama3 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama3));
            }
            if (silinecek.Tarama4 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama4));
            }
            if (silinecek.Tarama5 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama5));
            }
            if (silinecek.Tarama6 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama6));
            }
            if (silinecek.Tarama7 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama7));
            }
            if (silinecek.Tarama8 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama8));
            }
            if (silinecek.Tarama9 != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.Tarama9));
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
            var model = db.Ruhsatlar.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("RuhsatDetay", model);
        }

        private void DropDownDoldur(Ruhsatlar model)
        {
            List<RuhsatTipleri> ruhsatTipList = db.RuhsatTipleri.OrderBy(x => x.Tip).ToList();
            model.RuhsatTipListesi = (from x in ruhsatTipList
                                      select new SelectListItem
                                      {
                                          Text = x.Tip,
                                          Value = x.Id.ToString()
                                      }).ToList();


        }

        [Authorize(Roles = "A,RA,U")]
        public ActionResult DosyaSil(string File, Ruhsatlar p)
        {

            if (File == "file0")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
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
                var silinecek = db.Ruhsatlar.Find(p.Id);
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
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama2 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama2));
                    silinecek.Tarama2 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file3")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama3 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama3));
                    silinecek.Tarama3 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file4")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama4 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama4));
                    silinecek.Tarama4 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file5")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama5 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama5));
                    silinecek.Tarama5 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file6")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama6 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama6));
                    silinecek.Tarama6 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file7")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama7 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama7));
                    silinecek.Tarama7 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file8")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama8 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama8));
                    silinecek.Tarama8 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file9")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama9 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama9));
                    silinecek.Tarama9 = null;
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "proje")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
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
        public ActionResult TaramaSil(string File, Ruhsatlar p)
        {

            if (File == "file0")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
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
                var silinecek = db.Ruhsatlar.Find(p.Id);
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
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama2 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama2));
                    silinecek.Tarama2 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file3")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama3 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama3));
                    silinecek.Tarama3 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file4")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama4 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama4));
                    silinecek.Tarama4 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file5")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama5 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama5));
                    silinecek.Tarama5 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file6")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama6 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama6));
                    silinecek.Tarama6 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file7")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama7 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama7));
                    silinecek.Tarama7 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file8")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama8 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama8));
                    silinecek.Tarama8 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "file9")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
                if (silinecek.Tarama9 != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.Tarama9));
                    silinecek.Tarama9 = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (File == "proje")
            {
                var silinecek = db.Ruhsatlar.Find(p.Id);
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
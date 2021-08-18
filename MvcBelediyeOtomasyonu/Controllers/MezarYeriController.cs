using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MvcBelediyeOtomasyonu.Controllers;
using System.IO;
using PagedList;
using PagedList.Mvc;
using Microsoft.Reporting.WebForms;
using MvcBelediyeOtomasyonu;

namespace MvcBelediyeOtomasyonu.Controllers
{
   
    public class MezarYeriController : BaseController
    {
        // GET: MezarYeri
        [Authorize(Roles = "A,MA,U,MU")]
        public ActionResult Index(int sayfa = 1)
        {
            SiteBilgiGetir();
            var degerler = db.MezarYerleri.Include(x => x.Mezarliklar).Include(x => x.Adalar).ToList().ToPagedList(sayfa, 20);
            return View(degerler);
        }

        [Authorize(Roles = "A,MA,U,MU")]
        public ActionResult Search(int search, string p)
        {
            SiteBilgiGetir();
            var degerler = from d in db.MezarYerleri select d;
            if (!string.IsNullOrEmpty(p))
            {
                if (search == 1)
                {
                    //listBox1.Items.Clear();
                    int sayac = 1;
                    string ad = "", soyad = "";
                    char[] karakterler = p.ToCharArray();
                    foreach (char karakter in karakterler)
                    {
                        if (karakter.ToString() != " ")
                        {
                            ad += karakter.ToString();
                            sayac++;
                        }
                        else
                        {
                            soyad = p.Substring(sayac);
                            break;
                        }
                    }
                    degerler = degerler.Where(m => m.Adi.Contains(ad));
                    degerler = degerler.Where(m => m.Soyadi.Contains(soyad));
                    //degerler = degerler1;
                }
                else if (search == 2)
                {
                    degerler = degerler.Where(m => m.TcNo.Contains(p));
                }

            }
            return View(degerler.ToList());
        }

        [Authorize(Roles = "A,MA,U")]
        public ActionResult Yeni()
        {
            SiteBilgiGetir();
            var model = new MezarYerleri();
            Yenile(model);
            return View("MezarYeriForm", model);
        }

        private void Yenile(MezarYerleri model)
        {
            List<Mezarliklar> mezarlikList = db.Mezarliklar.OrderBy(x => x.Baslik).ToList();
            model.MezarlikListesi = (from x in mezarlikList
                                     select new SelectListItem
                                     {
                                         Text = x.Baslik,
                                         Value = x.Id.ToString()
                                     }).ToList();

            //List<Adalar> adaList = db.Adalar.Where(x=>x.Id == model.AdaId).OrderBy(x => x.AdaNo).ToList();
            //model.AdaListesi = (from x in adaList
            //                    select new SelectListItem
            //                    {
            //                        Text = x.AdaNo,
            //                        Value = x.Id.ToString()
            //                    }).ToList();

        }


        [ValidateAntiForgeryToken]
        [Authorize(Roles = "A,MA,U")]
        public ActionResult Kaydet(MezarYerleri p, HttpPostedFileBase MakbuzTarama)
        {
            if (!ModelState.IsValid)
            {
                var model = new MezarYerleri();
                Yenile(model);
                return View("Yeni", new MezarYerleri());
            }

            Random rastgele = new Random();
            int sayi0 = rastgele.Next(1000, 10000);

            if (p.Id == 0) //YENİ KAYIT İŞLEMİ
            {
                var mezarInDb = db.MezarYerleri.Where(x => x.MezarlikId == p.MezarlikId).Where(x => x.AdaId == p.AdaId).FirstOrDefault(x => x.MezarNo == p.MezarNo);

                if (mezarInDb == null)
                {
                    if (MakbuzTarama != null)
                    {
                        string dosyaadi = sayi0 + Path.GetFileName(MakbuzTarama.FileName);
                        string uzanti = Path.GetExtension(MakbuzTarama.FileName);
                        string yol = "~/makbuzlar/" + dosyaadi;
                        MakbuzTarama.SaveAs(Server.MapPath(yol));
                        p.MakbuzTarama = "/makbuzlar/" + dosyaadi;

                    }
                    p.SaveDate = DateTime.Now.ToString();
                    p.SaveUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                    db.Entry(p).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Mesaj = "Hata : '" + mezarInDb.Mezarliklar.Baslik + "'ndaki '" + mezarInDb.Adalar.AdaNo + "' numaralı ada üzerinde '" + mezarInDb.MezarNo + "' numaralı mezar yeri var...";
                    //return View("AdaForm", db.Adalar.ToList());
                    var model = new MezarYerleri();
                    Yenile(model);

                    List<Adalar> adaList = db.Adalar.Where(x => x.MezarlikId == mezarInDb.MezarlikId).OrderBy(x => x.AdaNo).ToList();
                    model.AdaListesi = (from x in adaList
                                        select new SelectListItem
                                        {
                                            Text = x.AdaNo,
                                            Value = x.Id.ToString()
                                        }).ToList();

                    //model.AdaId = p.AdaId;
                    return View("MezarYeriForm", model);
                }
            }
            else //GÜNCELLEME İŞLEMİ
            {
                string sonTarama0 = p.MakbuzTarama;


                if (MakbuzTarama != null)
                {
                    string dosyaadi = sayi0 + Path.GetFileName(MakbuzTarama.FileName);
                    string uzanti = Path.GetExtension(MakbuzTarama.FileName);
                    string yol = "~/makbuzlar/" + dosyaadi;
                    MakbuzTarama.SaveAs(Server.MapPath(yol));
                    p.MakbuzTarama = "/makbuzlar/" + dosyaadi;
                }
                else
                {
                    p.MakbuzTarama = sonTarama0;
                }
                p.EditDate = DateTime.Now.ToString();
                p.EditUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A,MA,U")]
        public ActionResult BilgiGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.MezarYerleri.Find(id);
            Yenile(model);
            List<Adalar> adaList = db.Adalar.Where(x => x.MezarlikId == model.MezarlikId).OrderBy(x => x.AdaNo).ToList();
            model.AdaListesi = (from x in adaList
                                select new SelectListItem
                                {
                                    Text = x.AdaNo,
                                    Value = x.Id.ToString()
                                }).ToList();



            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("MezarYeriForm", model);
        }

        [Authorize(Roles = "A,MA,U")]
        public ActionResult Sil(MezarYerleri p)
        {
            var silinecek = db.MezarYerleri.Find(p.Id);
            if (silinecek.MakbuzTarama != null)
            {
                System.IO.File.Delete(Server.MapPath(silinecek.MakbuzTarama));
            }
            db.Entry(silinecek).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A,MA,U,MU")]
        public ActionResult DetayGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.MezarYerleri.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("MezarYeriDetay", model);
        }

        [HttpPost]
        public JsonResult GetAda(int mezarlikIdParametre)
        {
            var model = new MezarYerleri();
            List<Adalar> adaList = db.Adalar.Where(x => x.MezarlikId == mezarlikIdParametre).OrderBy(x => x.AdaNo).ToList();
            model.AdaListesi = (from x in adaList
                                select new SelectListItem
                                {
                                    Text = x.AdaNo,
                                    Value = x.Id.ToString()
                                }).ToList();
            model.AdaListesi.Insert(0, new SelectListItem { Text = "---Seçiniz---", Value = "" });
            return Json(model.AdaListesi, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "A,MA,U")]
        public ActionResult DosyaSil(string File, MezarYerleri p)
        {

            if (File == "makbuztarama")
            {
                var silinecek = db.MezarYerleri.Find(p.Id);
                if (silinecek.MakbuzTarama != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.MakbuzTarama));
                    silinecek.MakbuzTarama = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("DetayGetir", new { id = p.Id });
        }

        [Authorize(Roles = "A,MA,U")]
        public ActionResult TaramaSil(string File, MezarYerleri p)
        {

            if (File == "makbuztarama")
            {
                var silinecek = db.MezarYerleri.Find(p.Id);
                if (silinecek.MakbuzTarama != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + silinecek.MakbuzTarama));
                    silinecek.MakbuzTarama = null;
                    silinecek.EditDate = DateTime.Now.ToString();
                    db.Entry(silinecek).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("BilgiGetir", new { id = p.Id });
        }

        [Authorize(Roles = "A,MA,U,MU")]
        public ActionResult DetailsReports(string ReportType, int Id)
        {
            MezarYerleriDataSetTableAdapters.MezarYerleriTableAdapter kayit = new MezarYerleriDataSetTableAdapters.MezarYerleriTableAdapter();//kayit tablosunda yaptığım için birleştirmeyi ve sql komutumu o yüzden adapter olarak kayit tablosunun örneğini getiriyorum.
            MezarYerleriDataSet.MezarYerleriDataTable mezarYeriTable = new MezarYerleriDataSet.MezarYerleriDataTable();// daha sonra hangi tabloda birleşeceği ve gösterileceğini o tablonun data table objesi ile getiriyorum.
            //kayit.Fill(kabinetDetayTable);
            kayit.Fill(mezarYeriTable, Id);//oluşturduğum unique isimi burada fill method olarak getiriyorum ve kendim belirlediğim kayıt numarasını direk yazıyorum.Daha sonra hangi data tableda oldugunu belirtmek için oluşturduğum kopya table ekliyorum.
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/MezarYeriRapor.rdlc");
            localreport.DataSources.Clear();//local report alanını siliyoruz bir öncekiler silinsin diye  var ise.
            ReportParameter rp = new ReportParameter("MezarYeriId", Id.ToString());
            localreport.SetParameters(rp);
            ReportDataSource reportDataSource1 = new ReportDataSource();
            reportDataSource1.Name = "MezarYeriDataSet";
            reportDataSource1.Value = mezarYeriTable.ToList();
            localreport.DataSources.Add(reportDataSource1);
            localreport.Refresh();
            //localreport.RefreshReport();
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;
            if (reportType == "Excel")
            {
                fileNameExtension = "xlsx";
            }
            else if (reportType == "Word")
            {
                fileNameExtension = "docx";
            }
            else if (reportType == "PDF")
            {
                fileNameExtension = "pdf";
            }
            else
            {
                fileNameExtension = "jpg";
            }
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment;filename=Switch24DetayRapor." + fileNameExtension);
            return File(renderedByte, mimeType);

        }

    }
}
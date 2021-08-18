using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;

namespace MvcBelediyeOtomasyonu.Controllers
{
    //[Authorize]
    
    public class AdalarController : BaseController
    {
        // GET: Adalar
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "A,U,MA,MU")]
        public ActionResult Index()
        {
            SiteBilgiGetir();
            return View(db.Adalar.Include(x => x.Mezarliklar).ToList());
        }
        [Authorize(Roles = "A,MA,U")]
        public ActionResult Yeni()
        {
            SiteBilgiGetir();

            var model = new Adalar();
            Yenile(model);
            return View("AdaForm", model);
        }

        private void Yenile(Adalar model)
        {
            List<Mezarliklar> mezarlikList = db.Mezarliklar.OrderBy(x => x.Baslik).ToList();
            model.MezarlikListesi = (from x in mezarlikList
                                     select new SelectListItem
                                     {
                                         Text = x.Baslik,
                                         Value = x.Id.ToString()
                                     }).ToList();

        }

        [Authorize(Roles = "A,MA,U")]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(Adalar p)
        {
            if (!ModelState.IsValid)
            {
                var model = new Adalar();
                Yenile(model);
                return View("Yeni");
            }


            if (p.Id == 0) //YENİ KAYIT İŞLEMİ
            {
                var adaInDb = db.Adalar.Where(x => x.MezarlikId == p.MezarlikId).FirstOrDefault(x => x.AdaNo == p.AdaNo);

                if (adaInDb == null)
                {
                    p.SaveDate = DateTime.Now.ToString();
                    p.SaveUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                    db.Entry(p).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    //return View("AdaForm");
                }
                else
                {
                    ViewBag.Mesaj = "Hata : '" + adaInDb.AdaNo + "' numaralı ada '" + adaInDb.Mezarliklar.Baslik + "' üzerinde kayıtlı...";
                    //return View("AdaForm", db.Adalar.ToList());
                    var model = new Adalar();
                    Yenile(model);
                    return View("AdaForm", model);
                }
            }
            else //GÜNCELLEME İŞLEMİ
            {
                p.EditDate = DateTime.Now.ToString();
                p.EditUserId = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();



                //    var adaInDb = db.Adalar.Where(x => x.MezarlikId == p.MezarlikId).FirstOrDefault(x => x.AdaNo == p.AdaNo);

                //    if (adaInDb.Id == p.Id)
                //    {
                //        db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //    else
                //    {
                //        ViewBag.Mesaj = "Hata : '" + adaInDb.AdaNo + "' numaralı ada '" + adaInDb.Mezarliklar.Baslik + "' üzerinde ZATEN kayıtlı...";
                //        //return View("AdaForm", db.Adalar.ToList());
                //        var model = new Adalar();
                //        Yenile(model);
                //        return View("AdaForm", model);
                //    }
                //}
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A,MA,U")]
        public ActionResult BilgiGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.Adalar.Find(id);
            Yenile(model);
            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("AdaForm", model);
        }

        [Authorize(Roles = "A,MA,U")]
        public ActionResult Sil(Adalar p)
        {
            var mezarYeriinDb = db.MezarYerleri.FirstOrDefault(x => x.AdaId == p.Id);

            if (mezarYeriinDb == null)
            {
                db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mesaj = "Hata : Seçilen ada içerisinde kayıtlı mezar yerleri var...";
                return View("Index", db.Adalar.ToList());
            }
        }

        [Authorize(Roles = "A,U,MA,MU")]
        public ActionResult DetayGetir(int id)
        {
            SiteBilgiGetir();
            var model = db.Adalar.Find(id);

            if (model == null)
            {
                return RedirectToAction("PageError");
            }
            return View("AdaDetay", model);
        }

        public int? DoluSay(int mezarliknumarasi, int adanumarasi)
        {
            var model = db.MezarYerleri.Where(x => x.MezarlikId == mezarliknumarasi).ToList();
            int doluSayi = model.Where(x => x.AdaId == adanumarasi).Count();
            return doluSayi;
        }

        public int? BosSay(int mezarliknumarasi, int adanumarasi)
        {
            var model = db.MezarYerleri.Where(x => x.MezarlikId == mezarliknumarasi).ToList();
            int doluSayi = model.Where(x => x.AdaId == adanumarasi).Count();
            var model1 = db.Adalar.Where(x => x.MezarlikId == mezarliknumarasi).FirstOrDefault(x => x.Id == adanumarasi);
            int mezarSayi = Convert.ToInt32(model1.MezarlikSayisi);
            int bosSayi = mezarSayi - doluSayi;
            return bosSayi;
        }

        public ActionResult DetailsReports(string ReportType, int Id)
        {
            MezarYerleriByAdaTableAdapters.MezarYerleriTableAdapter kayit = new MezarYerleriByAdaTableAdapters.MezarYerleriTableAdapter();
            MezarYerleriByAda.MezarYerleriDataTable mezarYerleribyAdaIdTable = new MezarYerleriByAda.MezarYerleriDataTable();
            kayit.Fill(mezarYerleribyAdaIdTable);
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/MezarYerleri.rdlc");
            localreport.DataSources.Clear();
            //ReportParameter rp = new ReportParameter("AdaId", Id.ToString());
            //localreport.SetParameters(rp);
            ReportDataSource reportDataSource1 = new ReportDataSource();
            reportDataSource1.Name = "MezarYerleribyAdaDS";
            reportDataSource1.Value = mezarYerleribyAdaIdTable.ToList();
            localreport.DataSources.Add(reportDataSource1);
            localreport.Refresh();
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
            return File(renderedByte, mimeType);

        }

    }
}
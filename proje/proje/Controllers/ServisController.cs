using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using proje.Models;
using proje.ViewModel;

namespace proje.Controllers
{
    public class ServisController : ApiController
    {
        DB01Entities db = new DB01Entities();
        SonucModel sonuc = new SonucModel();

        #region Dersler

        
        [HttpGet]
        [Route("api/derslistesi")]
        public List<DerslerModel> DersListesi()
        {
            List<DerslerModel> liste = db.Dersler.Select(x => new DerslerModel()
            {
                dersKodu = x.dersKodu,
                dersAdi = x.dersAdi,
                dersKredi = x.dersKredi,
                dersOgretimElemanı = x.dersOgretimElemanı,

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/dersbykod/{dersKodu}")]
        public DerslerModel DersById(string dersKodu)
        {
            DerslerModel kayit = db.Dersler.Where(s => s.dersKodu == dersKodu).Select(x => new DerslerModel()
            {
                dersKodu = x.dersKodu,
                dersAdi = x.dersAdi,
                dersKredi = x.dersKredi,
                dersOgretimElemanı = x.dersOgretimElemanı,

            }).FirstOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/dersekle")]
        public SonucModel DersEkle(DerslerModel model)
        {
            if (db.Dersler.Count(s => s.dersKodu == model.dersKodu) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Ders Kayıtlıdır!";
                return sonuc;
            }

            Dersler yeni = new Dersler();
            yeni.dersKodu = model.dersKodu;
            yeni.dersAdi = model.dersAdi;
            yeni.dersKredi = model.dersKredi;
            yeni.dersOgretimElemanı = model.dersOgretimElemanı;
            db.Dersler.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ders Eklendi";

            return sonuc;
        }

        [HttpPut]
        [Route("api/dersduzenle")]
        public SonucModel DersDuzenle(DerslerModel model)
        {
            Dersler kayit = db.Dersler.Where(s => s.dersKodu == model.dersKodu).FirstOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ders Bulunamadı";
                return sonuc;
            }

            kayit.dersAdi = model.dersAdi;
            kayit.dersKredi = model.dersKredi;
            kayit.dersOgretimElemanı = model.dersOgretimElemanı;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ders Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/derssil/{dersKodu}")]
        public SonucModel DersSil(string dersKodu)
        {
            Dersler kayit = db.Dersler.Where(s => s.dersKodu == dersKodu).FirstOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ders Bulunamadı";
                return sonuc;
            }

            if (db.Notlar.Count(s => s.notDersKodu == dersKodu) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Derse Kayıtlı Öğrenci Olduğu İçin Ders Silinemez";
                return sonuc;
            }

            db.Dersler.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ders Silindi";
            return sonuc;
        }

        #endregion

        #region Ogrenciler

        [HttpGet]
        [Route("api/ogrencilistesi")]

        public List<OgrencilerModel> OgrenciListesi()
        {
            List<OgrencilerModel> liste = db.Ogrenciler.Select(x => new OgrencilerModel()
            {
                ogrNo = x.ogrNo,
                ogrTC = x.ogrTC,
                ogrAd = x.ogrAd,
                ogrSoyad = x.ogrSoyad,
                ogrCinsiyet = x.ogrCinsiyet,
                ogrTelefon = x.ogrTelefon,
                ogrMail = x.ogrMail,
                ogrBolumKodu = x.ogrBolumKodu,
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/ogrencibyno/{ogrNo}")]

        public OgrencilerModel OgrenciById(string ogrNo)
        {
            OgrencilerModel kayit = db.Ogrenciler.Where(s => s.ogrNo == ogrNo).Select(x => new OgrencilerModel()
            {
                ogrNo = x.ogrNo,
                ogrTC = x.ogrTC,
                ogrAd = x.ogrAd,
                ogrSoyad = x.ogrSoyad,
                ogrCinsiyet = x.ogrCinsiyet,
                ogrTelefon = x.ogrTelefon,
                ogrMail = x.ogrMail,
                ogrBolumKodu = x.ogrBolumKodu,
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/ogrenciekle")]
        public SonucModel OgrenciEkle(OgrencilerModel model)
        {
            if (db.Ogrenciler.Count(s => s.ogrNo == model.ogrNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Öğrenci Numarası Kayıtlıdır!";
                return sonuc;
            }


            Ogrenciler yeni = new Ogrenciler();
            yeni.ogrNo = model.ogrNo;
            yeni.ogrTC = model.ogrTC;
            yeni.ogrAd = model.ogrAd;
            yeni.ogrSoyad = model.ogrSoyad;
            yeni.ogrCinsiyet = model.ogrCinsiyet;
            yeni.ogrTelefon = model.ogrTelefon;
            yeni.ogrMail = model.ogrMail;
            yeni.ogrBolumKodu = model.ogrBolumKodu;

            db.Ogrenciler.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/ogrenciduzenle")]
        public SonucModel OgrenciDuzenle(OgrencilerModel model)
        {
            Ogrenciler kayit = db.Ogrenciler.Where(s => s.ogrNo == model.ogrNo).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }


            kayit.ogrTC = model.ogrTC;
            kayit.ogrAd = model.ogrAd;
            kayit.ogrSoyad = model.ogrSoyad;
            kayit.ogrCinsiyet = model.ogrCinsiyet;
            kayit.ogrTelefon = model.ogrTelefon;
            kayit.ogrMail = model.ogrMail;
            kayit.ogrBolumKodu = model.ogrBolumKodu;           
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ogrencisil/{ogrNo}")]
        public SonucModel OgrenciSil(string ogrNo)
        {
            Ogrenciler kayit = db.Ogrenciler.Where(s => s.ogrNo == ogrNo).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Notlar.Count(s => s.notOgrenciNo == ogrNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Öğrenciye Ait Not Bulunduğu İçin Öğrenci Silinemez";
                return sonuc;
            }

            db.Ogrenciler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Silindi";
            return sonuc;
        }

        #endregion


        #region Bolumler

        [HttpGet]
        [Route("api/bolumlistesi")]

        public List<BolumlerModel> BolumListe()
        {
            List<BolumlerModel> liste = db.Bolumler.Select(x => new BolumlerModel()
            {
                bolumKodu = x.bolumKodu,
                bolumAdi = x.bolumAdi,
                bolumAdres = x.bolumAdres,
                bolumTelefon = x.bolumTelefon,
            }).ToList();

            return liste;
        }

        [HttpPost]
        [Route("api/bolumekle")]
        public SonucModel BolumEkle(BolumlerModel model)
        {
            if (db.Bolumler.Count(s => s.bolumKodu == model.bolumKodu) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Bölüm Kodu Kayıtlıdır!";
                return sonuc;
            }


            Bolumler yeni = new Bolumler();
            yeni.bolumKodu = model.bolumKodu;
            yeni.bolumAdi = model.bolumAdi;
            yeni.bolumAdres = model.bolumAdres;
            yeni.bolumTelefon = model.bolumTelefon;
            db.Bolumler.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Bölüm Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/bolumduzenle")]
        public SonucModel BolumDuzenle(BolumlerModel model)
        {
            Bolumler kayit = db.Bolumler.Where(s => s.bolumKodu == model.bolumKodu).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }


            kayit.bolumAdi = model.bolumAdi;
            kayit.bolumAdres = model.bolumAdres;
            kayit.bolumTelefon = model.bolumTelefon;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Bölüm Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/bolumsil/{bolumKodu}")]
        public SonucModel BolumSil(string bolumKodu)
        {
            Bolumler kayit = db.Bolumler.Where(s => s.bolumKodu == bolumKodu).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Ogrenciler.Count(s => s.ogrBolumKodu == bolumKodu) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bölüme Kayıtlı Öğrenci Olduğu İçin Bölüm Silinemez";
                return sonuc;
            }

            db.Bolumler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Bölüm Silindi";
            return sonuc;
        }

        #endregion

        #region Notlar

        [HttpGet]
        [Route("api/notlistesi")]

        public List<NotlarModel> NotListe()
        {
            List<NotlarModel> liste = db.Notlar.Select(x => new NotlarModel()
            {
                notOgrenciNo = x.notOgrenciNo,
                notDersKodu = x.notDersKodu,
                notVize = x.notVize,
                notFinal = x.notFinal,
                notOrtalama = x.notOrtalama,
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/ortalama/{notVize}/{notFinal}")]

        public Double Ortalama(int notVize, int notFinal)
        {
            double ort = (notVize * 0.4) + (notFinal * 0.6);
            return Math.Round(ort, 2);
        }

        [HttpPost]
        [Route("api/notekle")]
        public SonucModel NotEkle(NotlarModel model)
        {

            if (db.Notlar.Count(s => s.notOgrenciNo == model.notOgrenciNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Öğrenci'ye Ait Not Bulunmaktadır";
                return sonuc;
            }


            Notlar yeni = new Notlar();
            yeni.notOgrenciNo = model.notOgrenciNo;
            yeni.notDersKodu = model.notDersKodu;
            yeni.notVize = model.notVize;
            yeni.notFinal = model.notFinal;
            yeni.notOrtalama = model.notOrtalama;
            db.Notlar.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Not Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/notduzenle")]
        public SonucModel NotDuzenle(NotlarModel model)
        {
            Notlar kayit = db.Notlar.Where(s => s.notOgrenciNo == model.notOgrenciNo).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }


            kayit.notDersKodu = model.notDersKodu;
            kayit.notVize = model.notVize;
            kayit.notFinal = model.notFinal;
            kayit.notOrtalama = model.notOrtalama;
            
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Not Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/notsil/{notOgrenciNo}")]
        public SonucModel NotSil(string notOgrenciNo)
        {
            Notlar kayit = db.Notlar.Where(s => s.notOgrenciNo == notOgrenciNo).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Notlar.Count(s => s.notOgrenciNo == notOgrenciNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıtlı Öğrenci Olduğu İçin Not Silinemez";
                return sonuc;
            }

            db.Notlar.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Not Silindi";
            return sonuc;
        }

        #endregion
    }
}

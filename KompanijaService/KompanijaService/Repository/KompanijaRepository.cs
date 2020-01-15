using KompanijaService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KompanijaService.Models;

namespace KompanijaService.Repository
{
    public class KompanijaRepository : IDisposable, IKompanijaRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Kompanija> GetAll()
        {
            return db.Kompanije;
        }

        public Kompanija GetById(int id)
        {
            return db.Kompanije.SingleOrDefault(k => k.Id == id);
        }

        public IQueryable<KompanijaDTO> GetStatistika()
        {
            var zaposleni = db.Zaposleni.ToList();
            var kompanije = zaposleni.GroupBy(z => z.Kompanija, z => z.Plata, (kompanija, plata) =>
            new KompanijaDTO
            {
                Id = kompanija.Id,
                Naziv = kompanija.Naziv,
                GodinaOsnivanja = kompanija.GodinaOsnivanja,
                ProsecnaPlata = plata.Average()
            }).OrderByDescending(dto => dto.ProsecnaPlata);
            return kompanije.AsQueryable();
        }

        public IQueryable<Kompanija> GetTradicija()
        {
            List<Kompanija> kompanijeStarije = db.Kompanije.OrderBy(k => k.GodinaOsnivanja).ToList();
            List<Kompanija> kompanijeMladje = db.Kompanije.OrderByDescending(k => k.GodinaOsnivanja).ToList();
            List<Kompanija> dobijeneKompanije = new List<Kompanija>();
            dobijeneKompanije.Add(kompanijeStarije[0]);
            dobijeneKompanije.Add(kompanijeMladje[0]);
            return dobijeneKompanije.AsQueryable();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
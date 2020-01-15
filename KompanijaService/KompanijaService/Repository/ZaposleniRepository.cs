using KompanijaService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KompanijaService.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace KompanijaService.Repository
{
    public class ZaposleniRepository : IDisposable, IZaposleniRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void Add(Zaposleni zaposleni)
        {
            db.Zaposleni.Add(zaposleni);
            db.SaveChanges();
        }

        public void Delete(Zaposleni zaposleni)
        {
            db.Zaposleni.Remove(zaposleni);
            db.SaveChanges();
        }

        public IQueryable<Zaposleni> GetAll()
        {
            return db.Zaposleni.OrderByDescending(z => z.Plata);
        }

        public IQueryable<Zaposleni> GetByGodiste(int godiste)
        {
            return db.Zaposleni.Where(z => z.GodinaRodjenja > godiste).OrderBy(z => z.GodinaRodjenja);
        }

        public Zaposleni GetById(int id)
        {
            return db.Zaposleni.SingleOrDefault(z => z.Id == id);
        }

        public IQueryable<Zaposleni> GetByPretraga(ZaposleniFilter filter)
        {
            return db.Zaposleni.Where(z => z.GodinaZaposlenja >= filter.Pocetak && z.GodinaZaposlenja <= filter.Kraj).OrderBy(z => z.GodinaZaposlenja);
        }

        public void Update(Zaposleni zaposleni)
        {
            db.Entry(zaposleni).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }
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
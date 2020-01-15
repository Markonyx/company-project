using KompanijaService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompanijaService.Interface
{
    public interface IZaposleniRepository
    {
        IQueryable<Zaposleni> GetAll();
        Zaposleni GetById(int id);
        IQueryable<Zaposleni> GetByGodiste(int godiste);
        void Add(Zaposleni zaposleni);
        void Update(Zaposleni zaposleni);
        void Delete(Zaposleni zaposleni);
        IQueryable<Zaposleni> GetByPretraga(ZaposleniFilter filter);
    }
}

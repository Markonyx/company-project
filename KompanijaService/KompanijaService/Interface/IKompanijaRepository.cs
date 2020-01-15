using KompanijaService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompanijaService.Interface
{
    public interface IKompanijaRepository
    {
        IQueryable<Kompanija> GetAll();
        Kompanija GetById(int id);
        IQueryable<Kompanija> GetTradicija();
        IQueryable<KompanijaDTO> GetStatistika();
    }
}

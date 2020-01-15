using KompanijaService.Interface;
using KompanijaService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace KompanijaService.Controllers
{
    public class KompanijeController : ApiController
    {
        IKompanijaRepository _repository;

        public KompanijeController(IKompanijaRepository repository)
        {
            _repository = repository;
        }

        // GET api/kompanije
        public IQueryable<Kompanija> GetAll()
        {
            return _repository.GetAll();
        }

        // GET api/kompanije/1
        [ResponseType(typeof(Kompanija))]
        public IHttpActionResult GetKompanija(int id)
        {
            var kompanija = _repository.GetById(id);
            if(kompanija == null)
            {
                return NotFound();
            }
            return Ok(kompanija);
        }

        // GET api/tradicija
        [Route("api/tradicija")]
        public IQueryable<Kompanija> GetTradicija()
        {
            return _repository.GetTradicija();
        }

        // GET api/statistika
        [Route("api/statistika")]
        public IQueryable<KompanijaDTO> GetStatistika()
        {
            return _repository.GetStatistika();
        }
    }
}

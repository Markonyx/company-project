using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ZaposleniController : ApiController
    {
        IZaposleniRepository _repository;

        public ZaposleniController(IZaposleniRepository repository)
        {
            _repository = repository;
        }

        // GET api/zaposleni
        public IQueryable<ZaposleniDTO> GetAll()
        {
            var zaposleni = _repository.GetAll();
            return zaposleni.ProjectTo<ZaposleniDTO>();
        }

        // GET api/zaposleni/1
        [ResponseType(typeof(ZaposleniDTO))]
        public IHttpActionResult GetZaposleni(int id)
        {
            var zaposleni = _repository.GetById(id);
            if(zaposleni == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<ZaposleniDTO>(zaposleni));
        }

        // GET api/zaposleni/?godiste=1990
        public IQueryable<ZaposleniDTO> GetByGodiste(int godiste)
        {
            var dobijeniZaposleni = _repository.GetByGodiste(godiste);
            return dobijeniZaposleni.ProjectTo<ZaposleniDTO>();
        }

        // POST api/zaposleni
        [ResponseType(typeof(Zaposleni))]
        public IHttpActionResult PostZaposleni(Zaposleni zaposleni)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            _repository.Add(zaposleni);
            return CreatedAtRoute("DefaultApi", new { id = zaposleni.Id }, zaposleni);
        }

        // PUT api/zaposleni/1
        [ResponseType(typeof(Zaposleni))]
        [Authorize]
        public IHttpActionResult PutZaposleni(int id, Zaposleni zaposleni)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(id != zaposleni.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(zaposleni);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
            return Ok(zaposleni);
        }

        // DELETE api/zaposleni/1
        [ResponseType(typeof(void))]
        [Authorize]
        public IHttpActionResult DeleteZaposleni(int id)
        {
            var zaposleni = _repository.GetById(id);
            if(zaposleni == null)
            {
                return NotFound();
            }
            _repository.Delete(zaposleni);
            return Ok();
        }

        // POST api/zaposlenje
        [Route("api/zaposlenje")]
        [Authorize]
        public IQueryable<ZaposleniDTO> PostByZaposlenje(ZaposleniFilter filter)
        {
            var dobijeniZaposleni = _repository.GetByPretraga(filter);
            return dobijeniZaposleni.ProjectTo<ZaposleniDTO>();
        }
    }
}

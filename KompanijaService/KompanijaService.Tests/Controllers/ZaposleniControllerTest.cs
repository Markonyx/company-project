using AutoMapper;
using KompanijaService.Controllers;
using KompanijaService.Interface;
using KompanijaService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace KompanijaService.Tests.Controllers
{
    [TestClass]
    public class ZaposleniControllerTest
    {
        [TestMethod]
        public void GetReturnsZaposleniWithSameId()
        {
            // AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Zaposleni, ZaposleniDTO>();
            });

            // Arrange
            var mockRepository = new Mock<IZaposleniRepository>();
            mockRepository.Setup(x => x.GetById(42)).Returns(new Zaposleni { Id = 42 });

            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.GetZaposleni(42);
            var contentResult = actionResult as OkNegotiatedContentResult<ZaposleniDTO>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(42, contentResult.Content.Id);
        }

        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IZaposleniRepository>();
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.PutZaposleni(10, new Zaposleni { Id = 9, ImeIPrezime = "Probni zaposleni", GodinaRodjenja = 1980, GodinaZaposlenja = 2002, KompanijaId = 1 });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            // AutoMapper
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<Grad, GradDTO>();
            //});

            // Arrange
            List<Zaposleni> zaposleni = new List<Zaposleni>();
            Kompanija kompanija1 = new Kompanija();
            kompanija1.Naziv = "Kompanija1";
            kompanija1.GodinaOsnivanja = 1982;
            Kompanija kompanija2 = new Kompanija();
            kompanija2.Naziv = "Kompanija2";
            kompanija2.GodinaOsnivanja = 1983;
            zaposleni.Add(new Zaposleni { Id = 9, ImeIPrezime = "Probni zaposleni", GodinaRodjenja = 1980, GodinaZaposlenja = 2002, Kompanija = kompanija1 });
            zaposleni.Add(new Zaposleni { Id = 10, ImeIPrezime = "Probni zaposleni2", GodinaRodjenja = 1983, GodinaZaposlenja = 2001, Kompanija = kompanija2});

            List<ZaposleniDTO> dtoZaposleni = new List<ZaposleniDTO>();
            dtoZaposleni.Add(new ZaposleniDTO { Id = 9, ImeIPrezime = "Probni zaposleni", GodinaRodjenja = 1980, GodinaZaposlenja = 2002, KompanijaNaziv = "Kompanija1" });
            dtoZaposleni.Add(new ZaposleniDTO { Id = 10, ImeIPrezime = "Probni zaposleni2", GodinaRodjenja = 1983, GodinaZaposlenja = 2001, KompanijaNaziv = "Kompanija2" });

            var mockRepository = new Mock<IZaposleniRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(zaposleni.AsQueryable());
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IQueryable<ZaposleniDTO> result = controller.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dtoZaposleni.Count, result.ToList().Count);
            Assert.AreEqual(result.ElementAt(0).Id, dtoZaposleni.ElementAt(0).Id);
            Assert.AreEqual(result.ElementAt(1).Id, dtoZaposleni.ElementAt(1).Id);
        }

        [TestMethod]
        public void GetReturnsMultipleObjectsFilter()
        {
            // AutoMapper
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<Nekretnina, NekretninaDTO>();
            //});

            // Arrange
            List<Zaposleni> zaposleni = new List<Zaposleni>();
            ZaposleniFilter filter = new ZaposleniFilter() { Pocetak = 2008, Kraj = 2016 };
            Kompanija kompanija1 = new Kompanija();
            kompanija1.Naziv = "Kompanija1";
            kompanija1.GodinaOsnivanja = 1982;
            Kompanija kompanija2 = new Kompanija();
            kompanija2.Naziv = "Kompanija2";
            kompanija2.GodinaOsnivanja = 1983;
            zaposleni.Add(new Zaposleni { Id = 9, ImeIPrezime = "Probni zaposleni", GodinaRodjenja = 1980, GodinaZaposlenja = 2008, Kompanija = kompanija1 });
            zaposleni.Add(new Zaposleni { Id = 10, ImeIPrezime = "Probni zaposleni2", GodinaRodjenja = 1983, GodinaZaposlenja = 2016, Kompanija = kompanija2 });

            List<ZaposleniDTO> dtoZaposleni = new List<ZaposleniDTO>();
            dtoZaposleni.Add(new ZaposleniDTO { Id = 9, ImeIPrezime = "Probni zaposleni", GodinaRodjenja = 1980, GodinaZaposlenja = 2002, KompanijaNaziv = "Kompanija1" });
            dtoZaposleni.Add(new ZaposleniDTO { Id = 10, ImeIPrezime = "Probni zaposleni2", GodinaRodjenja = 1983, GodinaZaposlenja = 2001, KompanijaNaziv = "Kompanija2" });

            var mockRepository = new Mock<IZaposleniRepository>();
            mockRepository.Setup(x => x.GetByPretraga(filter)).Returns(zaposleni.AsQueryable());
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IQueryable<ZaposleniDTO> result = controller.PostByZaposlenje(filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dtoZaposleni.Count, result.ToList().Count);
            Assert.AreEqual(result.ElementAt(0).Id, dtoZaposleni.ElementAt(0).Id);
            Assert.AreEqual(result.ElementAt(1).Id, dtoZaposleni.ElementAt(1).Id);
        }
    }
}

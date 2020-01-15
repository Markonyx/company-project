namespace KompanijaService.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KompanijaService.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(KompanijaService.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Kompanije.AddOrUpdate(
                new Models.Kompanija() { Id = 1, Naziv = "Google", GodinaOsnivanja = 1998 },
                new Models.Kompanija() { Id = 2, Naziv = "Apple", GodinaOsnivanja = 1976 },
                new Models.Kompanija() { Id = 3, Naziv = "Microsoft", GodinaOsnivanja = 1975 }
                );
            context.SaveChanges();

            context.Zaposleni.AddOrUpdate(
                new Models.Zaposleni() { Id = 1, ImeIPrezime = "Pera Peric", GodinaRodjenja = 1980, GodinaZaposlenja = 2008, Plata = 3000M, KompanijaId = 1 },
                new Models.Zaposleni() { Id = 2, ImeIPrezime = "Mika Mikic", GodinaRodjenja = 1976, GodinaZaposlenja = 2005, Plata = 6000M, KompanijaId = 1 },
                new Models.Zaposleni() { Id = 3, ImeIPrezime = "Iva Ivic", GodinaRodjenja = 1990, GodinaZaposlenja = 2016, Plata = 4000M, KompanijaId = 2 },
                new Models.Zaposleni() { Id = 4, ImeIPrezime = "Zika Zikic", GodinaRodjenja = 1985, GodinaZaposlenja = 2005, Plata = 5000M, KompanijaId = 2 },
                new Models.Zaposleni() { Id = 1, ImeIPrezime = "Sara Saric", GodinaRodjenja = 1982, GodinaZaposlenja = 2007, Plata = 5500M, KompanijaId = 3 }
                );
            context.SaveChanges();
        }
    }
}

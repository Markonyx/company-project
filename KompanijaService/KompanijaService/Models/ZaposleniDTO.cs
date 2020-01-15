using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KompanijaService.Models
{
    public class ZaposleniDTO
    {
        public int Id { get; set; }
        public string ImeIPrezime { get; set; }
        public int GodinaRodjenja { get; set; }
        public int GodinaZaposlenja { get; set; }
        public decimal Plata { get; set; }
        public int KompanijaId { get; set; }
        public string KompanijaNaziv { get; set; }
    }
}
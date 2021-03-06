﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KompanijaService.Models
{
    public class Zaposleni
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string ImeIPrezime { get; set; }
        [Required]
        [Range(1952, 1990)]
        public int GodinaRodjenja { get; set; }
        [Range(2000, double.MaxValue)]
        public int GodinaZaposlenja { get; set; }
        [Required]
        [Range(2001, 9999)]
        public decimal Plata { get; set; }

        public int KompanijaId { get; set; }
        public virtual Kompanija Kompanija { get; set; }
    }
}
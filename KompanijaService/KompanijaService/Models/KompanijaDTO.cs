﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KompanijaService.Models
{
    public class KompanijaDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int GodinaOsnivanja { get; set; }
        public decimal ProsecnaPlata { get; set; }
    }
}
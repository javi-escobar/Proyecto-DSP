using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCate.Models
{
    public class Tour
    {
        public int TourID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Precio { get; set; }
        public int CantidadPersonas { get; set; }
    }
}
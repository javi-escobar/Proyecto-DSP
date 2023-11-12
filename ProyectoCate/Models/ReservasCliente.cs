using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCate.Models
{
    public class ReservasCliente
    {
        public int ClienteID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int CantidadDeReservas { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoCate
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservas
    {
        public int ReservaID { get; set; }
        public Nullable<int> ClienteID { get; set; }
        public Nullable<int> TourID { get; set; }
        public System.DateTime FechaReserva { get; set; }
        public decimal Descuento { get; set; }
        public virtual Clientes Clientes { get; set; }
        public virtual Tours Tours { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCate.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Correo {  get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
    }
}
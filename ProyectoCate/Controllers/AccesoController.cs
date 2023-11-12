using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ProyectoCate.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services.Description;

namespace ProyectoCate.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source=JAVI\\SQLEXPRESS;Initial Catalog=MiTour;Integrated Security=True";



        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario oUsuario)
        {
            bool registrado;
            string mensaje;

            if(oUsuario.Clave == oUsuario.ConfirmarClave)
            {
                oUsuario.Clave = ConvertirSha256(oUsuario.Clave);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena)) {
                SqlCommand cmd = new SqlCommand("RegistrarUsuario",cn);
                cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);

                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            ViewData["Mensaje"] = mensaje;

            if(registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {

            oUsuario.Clave = ConvertirSha256(oUsuario.Clave);

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);

                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                oUsuario.UsuarioId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }

            if(oUsuario.UsuarioId != 0)
            {
                Session["Usuario"] = oUsuario;
                return RedirectToAction("Index", "Clientes");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }

        }

        //Encriptación de Contraseña con SHA256
        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                foreach(byte b in result)
                {
                    Sb.Append(b.ToString("x2"));
                }
            }
            return Sb.ToString();
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoCate;
using System.Data.SqlClient;
using System.Web.Services.Description;
using ProyectoCate.Models;
using System.Drawing;
using ProyectoCate.Permisos;

namespace ProyectoCate.Controllers
{
    [ValidarSesion]
    public class ToursController : Controller
    {
        private MiTourEntities db = new MiTourEntities();
        static string cadena = "Data Source=JAVI\\SQLEXPRESS;Initial Catalog=MiTour;Integrated Security=True";

        // GET: Tours
        public async Task<ActionResult> Index()
        {
            return View(await db.Tours.ToListAsync());
        }

        // GET: Tours/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tours tours = await db.Tours.FindAsync(id);
            if (tours == null)
            {
                return HttpNotFound();
            }
            return View(tours);
        }

        // GET: Tours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tours/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TourID,Nombre,TipoTour,Nacional,Transporte,FechaInicio,FechaFin,CantidadPersonas,Precio")] Tours tours)
        {
            if (ModelState.IsValid)
            {
                db.Tours.Add(tours);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tours);
        }

        // GET: Tours/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tours tours = await db.Tours.FindAsync(id);
            if (tours == null)
            {
                return HttpNotFound();
            }
            return View(tours);
        }

        // POST: Tours/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TourID,Nombre,TipoTour,Nacional,Transporte,FechaInicio,FechaFin,CantidadPersonas,Precio")] Tours tours)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tours).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tours);
        }

        // GET: Tours/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tours tours = await db.Tours.FindAsync(id);
            if (tours == null)
            {
                return HttpNotFound();
            }
            return View(tours);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tours tours = await db.Tours.FindAsync(id);
            db.Tours.Remove(tours);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //Ver Torneos Disponibles
        public ActionResult ToursDisponibles()
        {
            List<Tour> toursDisponibles = ObtenerToursDisponibles();
            return View(toursDisponibles);
        }

        public List<Tour> ObtenerToursDisponibles()
        {
            List<Tour> tours = new List<Tour>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("VerToursDisponibles", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tour tour = new Tour
                        {
                            TourID = Convert.ToInt32(reader["TourID"]),
                            Nombre = reader["Nombre"].ToString(),
                            FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                            FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"])
                        };

                        tours.Add(tour);
                    }
                }
            }

            return tours;
        }

        //Ver Torneos Realizados
        public ActionResult ToursRealizados()
        {
            List<Tour> toursRealizados = ObtenerToursRealizados();
            return View(toursRealizados);
        }

        public List<Tour> ObtenerToursRealizados()
        {
            List<Tour> tours = new List<Tour>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("VerToursRealizados", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tour tour = new Tour
                        {
                            TourID = Convert.ToInt32(reader["TourID"]),
                            Nombre = reader["Nombre"].ToString(),
                            FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                            FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"])
                        };

                        tours.Add(tour);
                    }
                }
            }

            return tours;
        }

        //Ver Tours con Mayor Demanda
        public ActionResult ToursConMayorCantidadPersonas()
        {
            List<Tour> tours = ObtenerToursConMayorCantidadPersonas();
            return View(tours);
        }

        public List<Tour> ObtenerToursConMayorCantidadPersonas()
        {
            List<Tour> tours = new List<Tour>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("VerToursConMayorCantidadPersonas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tour tour = new Tour
                        {
                            TourID = Convert.ToInt32(reader["TourID"]),
                            Nombre = reader["Nombre"].ToString(),
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"])
                        };

                        tours.Add(tour);
                    }
                }
            }

            return tours;
        }

        //Tours con Menor Demanda
        public ActionResult ToursConMenorCantidadPersonas()
        {
            List<Tour> tours = ObtenerToursConMenorCantidadPersonas();
            return View(tours);
        }

        public List<Tour> ObtenerToursConMenorCantidadPersonas()
        {
            List<Tour> tours = new List<Tour>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("VerToursConMenorCantidadPersonas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tour tour = new Tour
                        {
                            TourID = Convert.ToInt32(reader["TourID"]),
                            Nombre = reader["Nombre"].ToString(),
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"])
                        };

                        tours.Add(tour);
                    }
                }
            }

            return tours;
        }

        //Cantidad de Resveras por Cliente y fechas
        public ActionResult CantidadDeReservasPorCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CantidadDeReservasPorCliente(int clienteID, DateTime fechaInicio, DateTime fechaFin)
        {
            if (!ClienteExiste(clienteID))
            {
                ViewBag.MensajeError = "El ID del Cliente ingresado no existe.";
                return View();
            }
            int cantidadDeReservas = ObtenerCantidadDeReservasPorCliente(clienteID, fechaInicio, fechaFin);

            ViewBag.ClienteID = clienteID; 
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.CantidadDeReservas = cantidadDeReservas;

            return View();
        }

        //Valida que el cliente id exista
        public bool ClienteExiste(int clienteID)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Clientes WHERE ClienteID = @ClienteID", cn);
                cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                cn.Open();

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public int ObtenerCantidadDeReservasPorCliente(int clienteID, DateTime fechaInicio, DateTime fechaFin)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("VerCantidadDeReservasPorCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                cn.Open();

                return (int)cmd.ExecuteScalar();
            }
        }

        //Ver los clientes con más reservas en un rango de fechas
        public ActionResult VerClientesConMasReservas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerClientesConMasReservas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ClientesConMasReservas> clientesConMasReservas = ObtenerClientesConMasReservas(fechaInicio, fechaFin);

            ViewBag.ClientesConMasReservas = clientesConMasReservas;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            return View();
        }


        // Método para obtener resultados del procedimiento almacenado
        private List<ClientesConMasReservas> ObtenerClientesConMasReservas(DateTime fechaInicio, DateTime fechaFin)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("VerClientesConMasReservas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    List<ClientesConMasReservas> resultados = new List<ClientesConMasReservas>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClientesConMasReservas cliente = new ClientesConMasReservas
                            {
                                ClienteID = Convert.ToInt32(reader["ClienteID"]),
                                Nombre = reader["Nombre"].ToString(),
                                CantidadDeReservas = Convert.ToInt32(reader["CantidadDeReservas"])
                            };

                            resultados.Add(cliente);
                        }
                    }

                    return resultados;
                }
            }
        }






    }
}

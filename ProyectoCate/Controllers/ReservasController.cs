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
using ProyectoCate.Permisos;

namespace ProyectoCate.Controllers
{
    [ValidarSesion]
    public class ReservasController : Controller
    {
        private MiTourEntities db = new MiTourEntities();

        // GET: Reservas
        public async Task<ActionResult> Index()
        {
            var reservas = db.Reservas.Include(r => r.Clientes).Include(r => r.Tours);
            return View(await reservas.ToListAsync());
        }

        // GET: Reservas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservas reservas = await db.Reservas.FindAsync(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            return View(reservas);
        }

        // GET: Reservas/Create
        public ActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nombre");
            ViewBag.TourID = new SelectList(db.Tours, "TourID", "Nombre");
            return View();
        }

        // POST: Reservas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReservaID,ClienteID,TourID,FechaReserva,Descuento")] Reservas reservas)
        {
            if (ModelState.IsValid)
            {
                db.Reservas.Add(reservas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nombre", reservas.ClienteID);
            ViewBag.TourID = new SelectList(db.Tours, "TourID", "Nombre", reservas.TourID);
            return View(reservas);
        }

        // GET: Reservas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservas reservas = await db.Reservas.FindAsync(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nombre", reservas.ClienteID);
            ViewBag.TourID = new SelectList(db.Tours, "TourID", "Nombre", reservas.TourID);
            return View(reservas);
        }

        // POST: Reservas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ReservaID,ClienteID,TourID,FechaReserva,Descuento")] Reservas reservas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservas).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nombre", reservas.ClienteID);
            ViewBag.TourID = new SelectList(db.Tours, "TourID", "Nombre", reservas.TourID);
            return View(reservas);
        }

        // GET: Reservas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservas reservas = await db.Reservas.FindAsync(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            return View(reservas);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Reservas reservas = await db.Reservas.FindAsync(id);
            db.Reservas.Remove(reservas);
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
    }
}

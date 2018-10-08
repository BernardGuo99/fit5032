using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mel_Medicare_Location_Reservation_System.Models;

namespace Mel_Medicare_Location_Reservation_System.Controllers
{
    public class EngagementsController : Controller
    {
        private MMLRS_Models db = new MMLRS_Models();

        // GET: Engagements
        public ActionResult Index(int id)
        {
            var engagements = db.Engagements.Where(e => e.reservationId == id);
            return View(engagements.ToList());
        }

        // GET: Engagements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Engagement engagement = db.Engagements.Find(id);
            if (engagement == null)
            {
                return HttpNotFound();
            }
            return View(engagement);
        }

        // GET: Engagements/Create
        public ActionResult Create(int id)
        {
            ViewBag.reservationId = new SelectList(db.Reservations.Where(r => r.reservationId == id), "reservationId", "reservationId");
            return View();
        }

        // POST: Engagements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,comment,generatedDate,initiator,reservationId")] Engagement engagement)
        {
            engagement.generatedDate = DateTime.Now;
            if (User.IsInRole("Customer"))
            {
                engagement.initiator = "Customer";
            }
            else
            {
                engagement.initiator = "Doctor";
            }

            ModelState.Clear();
            TryValidateModel(engagement);
            if (ModelState.IsValid)
            {
                db.Engagements.Add(engagement);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = engagement.reservationId});
            }

            ViewBag.reservationId = new SelectList(db.Reservations.Where(r => r.reservationId == engagement.reservationId), "reservationId", "reservationId", engagement.reservationId);
            return View(engagement);
        }

        // GET: Engagements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Engagement engagement = db.Engagements.Find(id);
            if (engagement == null)
            {
                return HttpNotFound();
            }
            ViewBag.reservationId = new SelectList(db.Reservations, "reservationId", "customerId", engagement.reservationId);
            return View(engagement);
        }

        // POST: Engagements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,comment,generatedDate,initiator,reservationId")] Engagement engagement)
        {
            engagement.generatedDate = DateTime.Now;
            ModelState.Clear();
            TryValidateModel(engagement);
            if (ModelState.IsValid)
            {
                db.Entry(engagement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = engagement.reservationId });
            }
            ViewBag.reservationId = new SelectList(db.Reservations, "reservationId", "customerId", engagement.reservationId);
            return View(engagement);
        }

        // GET: Engagements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Engagement engagement = db.Engagements.Find(id);
            if (engagement == null)
            {
                return HttpNotFound();
            }
            return View(engagement);
        }

        // POST: Engagements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Engagement engagement = db.Engagements.Find(id);
            //Engagement engagement1 = db.Engagements.Find(id);
            
            db.Engagements.Remove(engagement);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = engagement.reservationId });
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

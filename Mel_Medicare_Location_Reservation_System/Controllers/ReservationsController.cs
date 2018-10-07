using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_Week08A.Utils;
using Mel_Medicare_Location_Reservation_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Mel_Medicare_Location_Reservation_System.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private MMLRS_Models db = new MMLRS_Models();
        private ApplicationUserManager _userManager;

        public ReservationsController()
        {
        }

        public ReservationsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Reservations
        [Authorize(Roles = "Customer, Doctor")]
        public ActionResult Index()
        {
            
            if (User.IsInRole("Customer"))
            {
                var userName = User.Identity.Name;
                var reservations = db.Reservations.Where(r => r.customerId == userName);
                foreach (var res in reservations.ToList())
                {
                    if (res.date < DateTime.Now)
                    {
                        res.status = "Expired";

                    }
                }
                return View(reservations.ToList());
            }
            else
            {
                var reservations = db.Reservations;
                foreach (var res in reservations.ToList())
                {
                    if (res.date < DateTime.Now)
                    {
                        res.status = "Expired";
                    }
                }
                return View(reservations.ToList());
            }
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Customer, Doctor")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation.date < DateTime.Now)
            {
                reservation.status = "Expired";
            }
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "Customer")]
        public ActionResult Create(int? id)
        {
            ViewBag.branchId = new SelectList(db.Branches.Where(b => b.branchId == id), "branchId", "name");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "reservationId,branchId,customerId,date,status")] Reservation reservation)
        {
            String toEmail = UserManager.FindById(User.Identity.GetUserId()).Email;
            String subject = "Your New MHG Reservation";
            var branch = db.Branches.Where(b => b.branchId == reservation.branchId).FirstOrDefault();
            String contents = "Hello " + User.Identity.GetUserName() + ", \n your reservation to " + branch.name + " is on " + reservation.date + ". Please wait for your doctor's confirmation.";
            EmailSender es = new EmailSender();

            reservation.customerId = User.Identity.Name;
            reservation.status = "Pending";
            ModelState.Clear();
            TryValidateModel(reservation);


            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                es.Send(toEmail, subject, contents);
                return RedirectToAction("ReservationSucceed");
            }

            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }

        public ActionResult ReservationSucceed()
        {
            return View();
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Doctor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reservationId,branchId,customerId,date,status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = UserManager.FindByName(db.Reservations.Find(id).customerId);
            String toEmail = user.Email;
            String subject = "Cancellation of Your MHG Reservation";
            var res = db.Reservations.Where(r => r.reservationId == id).FirstOrDefault();
            var branch = db.Branches.Where(b => b.branchId == res.branchId).FirstOrDefault();
            String contents = "Hello " + user.UserName + ", your reservation to " + branch.name + " on " + res.date + " has been cancelled";
            EmailSender es = new EmailSender();

            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();

            es.Send(toEmail, subject, contents);

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

        [Authorize(Roles = "Doctor")]
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve([Bind(Include = "reservationId,branchId,customerId,date,status")] Reservation reservation)
        {
            String toEmail = UserManager.FindByName(reservation.customerId).Email;
            String subject = "Approvement of Your MHG Reservation";
            var branch = db.Branches.Where(b => b.branchId == reservation.branchId).FirstOrDefault();
            String contents = "Hello " + reservation.customerId + ", \n your reservation to " + branch.name + " on " + reservation.date + " has been confirmed by your doctor.";
            EmailSender es = new EmailSender();
            reservation.status = "Approved";


            ModelState.Clear();
            TryValidateModel(reservation);
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                es.Send(toEmail, subject, contents);
                return RedirectToAction("Index");
            }
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }


        [Authorize(Roles = "Doctor")]
        public ActionResult Reschedule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);

            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reschedule([Bind(Include = "reservationId,branchId,customerId,date,status")] Reservation reservation)
        {
            String toEmail = UserManager.FindByName(reservation.customerId).Email;
            String subject = "Reschedule of Your MHG Reservation";
            var branch = db.Branches.Where(b => b.branchId == reservation.branchId).FirstOrDefault();
            String contents = "Hello " + reservation.customerId + ", \n your reservation to " + branch.name + " has been rescheduled to " + reservation.date;
            EmailSender es = new EmailSender();


            reservation.status = "Approved";


            ModelState.Clear();
            TryValidateModel(reservation);
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                es.Send(toEmail, subject, contents);
                return RedirectToAction("Index");
            }
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }

        [Authorize(Roles = "Doctor, Customer")]
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);

            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel([Bind(Include = "reservationId,branchId,customerId,date,status")] Reservation reservation)
        {
            String toEmail = UserManager.FindByName(reservation.customerId).Email;
            String subject = "Cancellation of Your MHG Reservation";
            var branch = db.Branches.Where(b => b.branchId == reservation.branchId).FirstOrDefault();
            String contents = "Hello " + reservation.customerId + ", \n your reservation to " + branch.name + " on " + reservation.date + " has been cancelled.";
            EmailSender es = new EmailSender();
            reservation.status = "Cancelled";


            ModelState.Clear();
            TryValidateModel(reservation);
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                es.Send(toEmail, subject, contents);
                return RedirectToAction("Index");
            }
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name", reservation.branchId);
            return View(reservation);
        }

    }
}

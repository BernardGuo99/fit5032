using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FIT5032_Week08A.Utils;
using Mel_Medicare_Location_Reservation_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Mel_Medicare_Location_Reservation_System.Controllers
{
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
        [Authorize(Roles = "Customer, Admin")]
        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                var userName = User.Identity.Name;
                var reservations = db.Reservations.Where(r => r.customerId == userName);
                return View(reservations.ToList());
            }
            else
            {
                var reservations = db.Reservations;
                return View(reservations.ToList());
            }
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
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

        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.branchId = new SelectList(db.Branches, "branchId", "name");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "reservationId,branchId,customerId,date")] Reservation reservation)
        {
            String toEmail = UserManager.FindById(User.Identity.GetUserId()).Email;
            String subject = "Your New MHG Reservation";
            var branch = db.Branches.Where(b => b.branchId == reservation.branchId).FirstOrDefault();
            String contents = "Hello " + User.Identity.GetUserName() + ", your reservation to " + branch.name + " is on " + reservation.date;
            EmailSender es = new EmailSender();
            
            reservation.customerId = User.Identity.Name;
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
        public ActionResult Edit([Bind(Include = "reservationId,branchId,customerId,date")] Reservation reservation)
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


        


    }
}

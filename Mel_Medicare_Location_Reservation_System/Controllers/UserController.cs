using Mel_Medicare_Location_Reservation_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mel_Medicare_Location_Reservation_System.Controllers
{
    public class UserController : Controller
    {
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager)
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



        // GET: User
        public ActionResult Index()
        {
            ViewBag.Message = "Your contact page.";

            var UsersContext = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (roleManager.RoleExists("Customer"))
            {
                var role = roleManager.FindByName("Customer").Users.FirstOrDefault();
                var usersInRole = UsersContext.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.RoleId)).ToList();
                return View(usersInRole);
            }
            else
            {
                var emptyList = new List<ApplicationUser>();
                return View(emptyList);
            }
        }

        public ActionResult Details(string id)
        {
            var user = UserManager.FindByIdAsync(id).Result;
            return View(user);
        }

        public ActionResult Edit(string id)
        {
            var user = UserManager.FindByIdAsync(id).Result;
            return View(user);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel AppUserViewModel)
        {
            ApplicationUser model = UserManager.FindById(AppUserViewModel.Id);
            
            model.Email = AppUserViewModel.Email;
            model.PhoneNumber = AppUserViewModel.PhoneNumber;
            model.UserName = AppUserViewModel.UserName;
            //var user = new ApplicationUser() { Email = applicationUser.Email, PhoneNumber = applicationUser.PhoneNumber, UserName = applicationUser.UserName };
            await UserManager.UpdateAsync(model);
            return RedirectToAction("Index");
            
            //await _userManager.UpdateAsync(model);
            //return RedirectToAction("Index");
        }



        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            await UserManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }





    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdiManager.Data;
using PdiManager.Models;

namespace PdiManager.Controllers
{
    public class PdiController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        
        public PdiController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _db = dbContext;
            _userManager = userManager;
        }
        // GET
        public async Task<IActionResult> Index()
        {
            List<Pdi> pdis;
            if (User.HasClaim(c => c.Value == "manager"))
            {
                pdis = _db
                    .Pdis
                    .Include("User")
                    .Include("Tasks")
                    .ToList();
                return View(pdis);
            }

            var user = await _userManager.GetUserAsync(User);
            pdis = _db
                .Pdis
                .Where(p => p.User.Id == user.Id)
                .Include("User")
                .Include("Tasks")
                .ToList();
            return View(pdis);
        }

        public IActionResult Edit(int id)
        {
            var Pdi = _db.Pdis.Include("Tasks").First(p => p.Id == id);
            ViewData["Users"] = _userManager.Users.ToList();
            return View(Pdi);
        }

        public IActionResult Finish(int id)
        {
            var Pdi = _db.Pdis.First(p => p.Id == id);
            Pdi.IsDone = true;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            if (User.HasClaim(c => c.Value == "manager"))
            {
                ViewData["Users"] = _userManager.Users.ToList();
            }
            else
            {
                ViewData["Users"] = _userManager.Users.Where(u => u.UserName == User.Identity.Name).ToList();
            }
            return View("Edit", new Pdi());
        }
        
        [HttpPost]
        public IActionResult Store(Pdi pdi)
        {
            Console.WriteLine(pdi.Id);
            pdi.User = _db.Users.First(u => u.Id == pdi.User.Id);
            if (pdi.Id == 0)
            {
                _db.Pdis.Add(pdi);
            }
            else
            {
                Pdi Pdi = _db.Pdis.First(registro => registro.Id == pdi.Id);
                Pdi.Name = pdi.Name;
                Pdi.EndAt = pdi.EndAt;
                Pdi.StartAt = pdi.StartAt;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
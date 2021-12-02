using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PdiManager.Data;
using PdiManager.Models;

namespace PdiManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        
        public HomeController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _db = dbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if (User.HasClaim(c => c.Value == "manager"))
            {
                int openPdis = _db
                        .Pdis
                        .Count(p => !p.IsDone);
                int nextPdis = _db
                    .Pdis.Where(p => !p.IsDone)
                    .Count(p => p.EndAt.CompareTo(DateTime.Now.AddMonths(1)) < 0);
                ViewData["OpenPdis"] = openPdis;
                ViewData["NextPdis"] = nextPdis;
                return View("Manager");                
            }
            if (User.HasClaim(c => c.Value == "employee"))
            {
                ViewData["OpenTasks"] = 0;
                ViewData["NextReview"] = 0;
                Pdi openTasks;
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    openTasks = _db
                        .Pdis
                        .Include("Tasks")
                        .Include("User")
                        .First(p => p.User.Id == user.Id && !p.IsDone);
                    
                    if (openTasks != null)
                    {
                        ViewData["OpenTasks"] = openTasks.Tasks.Count(t => !t.IsDone);
                        ViewData["NextReview"] = openTasks.EndAt.Date.ToString("d");   
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
                }
                return View("Employee");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

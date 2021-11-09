using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PdiManager.Controllers
{
    public class EmployeeController : Controller
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        
        public EmployeeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            var employee = await _userManager.GetUsersForClaimAsync(new Claim("role", "employee"));
            ViewData["IsManager"] = User.HasClaim(c => c.Value == "manager"); 
            return View(employee);
        }

        public async Task<IActionResult> MakeManager(string id)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id.Equals(id));
            
            await _userManager.ReplaceClaimAsync(user, new Claim("Role", "employee"), new Claim("Role", "manager"));
            return RedirectToAction("Index");
        }
    }
}
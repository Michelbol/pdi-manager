using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace PdiManager.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        
        public ManagerController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        // GET
        public async Task<IActionResult> Index()
        {
            var managers = await _userManager.GetUsersForClaimAsync(new Claim("role", "manager"));
            ViewData["IsManager"] = User.HasClaim(c => c.Value == "manager");
            return View(managers);
        }
        
        public async Task<IActionResult> MakeEmployee(string id)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id.Equals(id));
            
            await _userManager.ReplaceClaimAsync(user, new Claim("Role", "manager"), new Claim("Role", "employee"));
            return RedirectToAction("Index");
        }
    }
}
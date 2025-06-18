using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.Where(e => e.IsApproved).ToListAsync();
            return View(events);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
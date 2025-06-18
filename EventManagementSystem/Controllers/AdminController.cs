using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.Where(e => !e.IsApproved).ToListAsync();
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            @event.IsApproved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
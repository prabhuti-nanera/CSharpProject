using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.Controllers
{
    [Authorize(Roles = "Organizer")] // Keep if adding authentication
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    @event.OrganizerId = user?.Id ?? throw new InvalidOperationException("User not found");
                    @event.IsApproved = false;
                    _context.Add(@event);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Error saving event: " + ex.InnerException?.Message);
                }
            }
            return View(@event);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (@event.OrganizerId != user?.Id) return Unauthorized();
                _context.Update(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            if (@event.OrganizerId != user?.Id) return Unauthorized();
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

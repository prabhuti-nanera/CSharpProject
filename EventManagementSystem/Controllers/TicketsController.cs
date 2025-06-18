using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;
using EventManagementSystem.Models;
using QRCoder;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementSystem.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Purchase(int? eventId)
        {
            if (eventId == null) return NotFound();
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId && e.IsApproved);
            if (@event == null) return NotFound();
            ViewBag.EventId = eventId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int eventId)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId && e.IsApproved);
            if (@event == null) return NotFound();
            var ticketCount = await _context.Tickets.CountAsync(t => t.EventId == eventId);
            if (ticketCount >= @event.MaxTickets)
            {
                ModelState.AddModelError("", "No tickets available.");
                ViewBag.EventId = eventId;
                return View();
            }

            var ticket = new Ticket
            {
                EventId = eventId,
                UserId = User.Identity.Name,
                QRCode = GenerateQRCode($"{eventId}-{User.Identity.Name}")
            };
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Email sent to {User.Identity.Name} for event {@event.Title} with QR code.");
            return RedirectToAction("PurchaseSuccess");
        }

        public IActionResult PurchaseSuccess()
        {
            return View();
        }

        private string GenerateQRCode(string data)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);
            return Convert.ToBase64String(qrCodeImage);
        }
    }
}
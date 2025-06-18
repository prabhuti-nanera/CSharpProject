using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }
        [Required]
        public string UserId { get; set; }
        public string QRCode { get; set; }
    }
}
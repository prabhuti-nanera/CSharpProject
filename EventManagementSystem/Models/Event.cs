using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int MaxTickets { get; set; }
        public bool IsApproved { get; set; }
        public string OrganizerId { get; set; }
    }
}
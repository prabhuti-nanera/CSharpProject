using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public bool IsApproved { get; set; }
        public int MaxTickets { get; set; }
        public string OrganizerId { get; set; }
        public string Description { get; set; }
    }
}
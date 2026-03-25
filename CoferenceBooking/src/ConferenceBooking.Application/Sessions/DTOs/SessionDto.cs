using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceBooking.Application.DTOs
{
    public class SessionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentBookings { get; set; } // To show how many bookings are currently active
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
    }
    
    public class SessionDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentBookings { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
    }
    
    public class CreateSessionDto
    {
        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "MaxCapacity must be greater than 0")]
        public int MaxCapacity { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        [Required]
        public string Location { get; set; }
    }
}
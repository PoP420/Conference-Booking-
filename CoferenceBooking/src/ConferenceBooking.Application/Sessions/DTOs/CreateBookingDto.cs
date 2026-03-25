using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceBooking.Application.DTOs
{
    public class CreateBookingDto
    {
        public Guid SessionId { get; set; }
    }
    
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string SessionTitle { get; set; }
        public DateTime BookingTime { get; set; }
        public string Status { get; set; }
    }
    
    public class FilterDto
    {
        public string? Title { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Location { get; set; }
    }
}
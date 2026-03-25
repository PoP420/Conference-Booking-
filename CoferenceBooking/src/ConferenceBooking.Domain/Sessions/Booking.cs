using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ConferenceBooking.Domain.Sessions
{
    public class Booking : Entity<Guid>
    {
        public Guid SessionId { get; private set; }
        public Guid UserId { get; private set; }           // As specified in section 1.2 and 1.3
        public DateTime BookingTime { get; private set; }  // As specified in section 1.2
        public string Status { get; private set; }         // As specified in section 1.2
        
        // Navigation property for the session
        public virtual Session Session { get; private set; }
        
        // UserId represents the relationship with User (as per section 1.3)

        // Private constructor for ORM
        private Booking()
        {
        }

        public Booking(
            Guid id,
            Guid sessionId,
            Guid userId,
            string status)
        {
            Id = id;
            SessionId = sessionId;
            UserId = userId;
            BookingTime = DateTime.Now;
            SetStatus(status);
        }

        public void SetStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be null or empty.");
            
            // Business rule: Only allow valid statuses
            if (status != "Confirmed" && status != "Cancelled")
            {
                throw new BusinessException($"Invalid status: {status}. Status must be either 'Confirmed' or 'Cancelled'.");
            }
            
            Status = status;
        }
        
        public bool IsConfirmed => Status == "Confirmed";
        public bool IsCancelled => Status == "Cancelled";
    }
}
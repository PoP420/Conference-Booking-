using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ConferenceBooking.Domain.Sessions
{
    public class Session : AggregateRoot<Guid>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int MaxCapacity { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string Location { get; private set; }
        
        // Navigation: Session has a collection of Bookings (as per section 1.3)
        public virtual ICollection<Booking> Bookings { get; protected set; }

        // Private constructor for ORM
        private Session()
        {
            Bookings = new List<Booking>();
        }

        public Session(
            Guid id,
            string title,
            string description,
            int maxCapacity,
            DateTime startTime,
            DateTime endTime,
            string location)
            : base(id)
        {
            // Validate before setting values to avoid issues with individual setters checking against default values
            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time.");
            
            SetTitle(title);
            SetDescription(description);
            SetMaxCapacity(maxCapacity);
            
            // Direct assignment to bypass validation logic that depends on the other field being already set
            StartTime = startTime;
            EndTime = endTime;
            
            SetLocation(location);
            
            Bookings = new List<Booking>();
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.");
            
            Title = title;
        }

        public void SetDescription(string description)
        {
            Description = description ?? string.Empty;
        }

        public void SetMaxCapacity(int maxCapacity)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException("MaxCapacity must be greater than zero.");
                
            MaxCapacity = maxCapacity;
        }

        public void SetStartTime(DateTime startTime)
        {
            if (startTime >= EndTime)
                throw new ArgumentException("Start time must be before end time.");
                
            StartTime = startTime;
        }

        public void SetEndTime(DateTime endTime)
        {
            if (endTime <= StartTime)
                throw new ArgumentException("End time must be after start time.");
                
            EndTime = endTime;
        }

        public void SetLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be null or empty.");
                
            Location = location;
        }

        // Business method to check if session is full
        public bool IsFull => GetCurrentBookingCount() >= MaxCapacity;

        // Business method to get current booking count
        public int GetCurrentBookingCount()
        {
            // Count bookings that are not cancelled (only confirmed bookings count toward capacity)
            return Bookings.Count(b => b.IsConfirmed);
        }

        // Business method to check if booking is possible
        public bool CanAcceptNewBooking()
        {
            return !IsFull && StartTime > DateTime.Now;
        }
        
        // Business Rule 1: Capacity Constraint - Session.Bookings.Count cannot exceed Session.MaxCapacity
        private bool CanExceedCapacity(int additionalBookings = 1)
        {
            return (GetCurrentBookingCount() + additionalBookings) <= MaxCapacity;
        }
        
        // Business Rule 2: Time Constraint - Booking cannot occur if DateTime.UtcNow > Session.StartTime
        private bool IsBeforeSessionStart()
        {
            return DateTime.Now < StartTime;
        }
        
        // Business Rule 3: Duplicate Constraint - A User cannot have more than one Booking per Session
        public bool HasUserAlreadyBooked(Guid userId)
        {
            return Bookings.Any(b => b.UserId == userId && b.IsConfirmed);
        }
        
        // Main method to create a booking with all business rule validations
        public Booking CreateBooking(Guid bookingId, Guid userId)
        {
            // Validate all business rules before creating a booking
            
            // Check time constraint: cannot book after session has started
            if (!IsBeforeSessionStart())
            {
                throw new BusinessException(message: "Cannot book a session after it has started.");
            }
            
            // Check capacity constraint: cannot exceed max capacity
            if (!CanExceedCapacity(1))
            {
                throw new BusinessException(message: "Cannot book a session. Maximum capacity reached.");
            }
            
            // Check duplicate constraint: user cannot book the same session twice
            if (HasUserAlreadyBooked(userId))
            {
                throw new BusinessException(message: "User has already booked this session.");
            }
            
            // Create and add the booking
            var booking = new Booking(bookingId, this.Id, userId, "Confirmed");
            Bookings.Add(booking);
            return booking;
        }
        
        // Method to cancel a booking
        public void CancelBooking(Guid bookingId)
        {
            var booking = Bookings.FirstOrDefault(b => b.Id == bookingId && b.IsConfirmed);
            if (booking != null)
            {
                booking.SetStatus("Cancelled");
            }
        }
    }
}
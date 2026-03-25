using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceBooking.Application.DTOs;
using ConferenceBooking.Domain.Sessions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.Identity;

namespace ConferenceBooking.Application.Sessions
{
    public class SessionAppService : ApplicationService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IRepository<Booking, Guid> _bookingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserAppService _identityUserAppService;

        public SessionAppService(
            ISessionRepository sessionRepository,
            IRepository<Booking, Guid> bookingRepository,
            ICurrentUser currentUser,
            IIdentityUserAppService identityUserAppService)
        {
            _sessionRepository = sessionRepository;
            _bookingRepository = bookingRepository;
            _currentUser = currentUser;
            _identityUserAppService = identityUserAppService;
        }

        // UC-01: View Sessions
        public async Task<List<SessionDto>> GetListAsync(FilterDto input = null)
        {
            var sessionsQuery = await _sessionRepository.GetListAsync();
            
            // Apply filters if provided
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input.Title))
                    sessionsQuery = sessionsQuery.Where(s => s.Title.Contains(input.Title)).ToList();
                    
                if (input.FromDate.HasValue)
                    sessionsQuery = sessionsQuery.Where(s => s.StartTime >= input.FromDate.Value).ToList();
                    
                if (input.ToDate.HasValue)
                    sessionsQuery = sessionsQuery.Where(s => s.StartTime <= input.ToDate.Value).ToList();
                    
                if (!string.IsNullOrEmpty(input.Location))
                    sessionsQuery = sessionsQuery.Where(s => s.Location.Contains(input.Location)).ToList();
            }

            var dtos = ObjectMapper.Map<List<Session>, List<SessionDto>>(sessionsQuery);
            
            // Set current bookings count for each session
            foreach (var dto in dtos)
            {
                var session = sessionsQuery.First(s => s.Id == dto.Id);
                dto.CurrentBookings = session.GetCurrentBookingCount();
            }

            return dtos;
        }

        // UC-02: View Session Details
        public async Task<SessionDetailDto> GetAsync(Guid id)
        {
            var session = await _sessionRepository.GetAsync(id);
            
            var dto = ObjectMapper.Map<Session, SessionDetailDto>(session);
            dto.CurrentBookings = session.GetCurrentBookingCount();
            
            return dto;
        }

        // UC-03: Book Session
        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto input)
        {
            var session = await _sessionRepository.GetAsync(input.SessionId);
            
            // Create booking with current user
            var booking = session.CreateBooking(
                Guid.NewGuid(), 
                _currentUser.Id!.Value
            );
            
            // Save the session with the new booking
            await _sessionRepository.UpdateAsync(session);
            
            // Map and return booking DTO
            var bookingDto = new BookingDto
            {
                Id = booking.Id,
                SessionId = booking.SessionId,
                SessionTitle = session.Title,
                BookingTime = booking.BookingTime,
                Status = booking.Status
            };
            
            return bookingDto;
        }

        // UC-04: Cancel Booking
        public async Task CancelBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            
            // Verify that the current user owns this booking
            if (booking.UserId != _currentUser.Id)
            {
                throw new Volo.Abp.Authorization.AbpAuthorizationException("Not authorized to cancel this booking.");
            }
            
            // Find the session and cancel the booking
            var session = await _sessionRepository.GetAsync(booking.SessionId);
            session.CancelBooking(bookingId);
            
            await _sessionRepository.UpdateAsync(session);
        }

        // UC-05: View My Bookings
        public async Task<List<BookingDto>> GetMyBookingsAsync()
        {
            var bookings = await _bookingRepository.GetListAsync(
                b => b.UserId == _currentUser.Id!.Value);

            var bookingDtos = new List<BookingDto>();
            
            foreach (var booking in bookings)
            {
                // Get the session for the booking to get the title
                var session = await _sessionRepository.GetAsync(booking.SessionId);
                
                bookingDtos.Add(new BookingDto
                {
                    Id = booking.Id,
                    SessionId = booking.SessionId,
                    SessionTitle = session.Title,
                    BookingTime = booking.BookingTime,
                    Status = booking.Status
                });
            }
            
            return bookingDtos;
        }

        // UC-06: Create Session
        public async Task<SessionDto> CreateSessionAsync(CreateSessionDto input)
        {
            var session = new Session(
                Guid.NewGuid(),
                input.Title,
                input.Description,
                input.MaxCapacity,
                input.StartTime,
                input.EndTime,
                input.Location
            );

            var createdSession = await _sessionRepository.InsertAsync(session);
            
            var dto = ObjectMapper.Map<Session, SessionDto>(createdSession);
            dto.CurrentBookings = createdSession.GetCurrentBookingCount();
            
            return dto;
        }
    }
}
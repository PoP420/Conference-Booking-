using System;
using System.Linq;
using Reqnroll;
using ConferenceBooking.Domain.Sessions;
using Xunit;
using Shouldly;

namespace ConferenceBooking.Domain.Tests.Features
{
    [Binding]
    public class SessionBookingSteps 
    {
        private Session session;
        private Guid[] bookedUsers;
        private int userCount;
        private Exception caughtException;
        private Guid newUserId;
        private int initialBookingCount;

        [Given(@"a session ""([^""]*)"" with capacity (\d+)")]
        public void GivenASessionWithCapacity(string title, int capacity)
        {
            var startTime = DateTime.Now.AddDays(1);
            var endTime = startTime.AddHours(2); // Ensure end time is after start time
            
            session = new Session(
                Guid.NewGuid(),
                title,
                $"Description for {title}",
                capacity,
                startTime,
                endTime,
                "Main Hall"
            );
            bookedUsers = new Guid[capacity];
            userCount = 0;
        }

        [Given(@"(\d+) users have already booked the session")]
        public void GivenUsersHaveAlreadyBookedTheSession(int count)
        {
            initialBookingCount = session.GetCurrentBookingCount();
            
            for (int i = 0; i < count; i++)
            {
                var userId = Guid.NewGuid();
                session.CreateBooking(Guid.NewGuid(), userId);
                bookedUsers[i] = userId;
                userCount++;
            }
            
            session.GetCurrentBookingCount().ShouldBe(initialBookingCount + count);
        }

        [Given(@"a user has already booked this session")]
        public void GivenAUserHasAlreadyBookedThisSession()
        {
            var userId = Guid.NewGuid();
            session.CreateBooking(Guid.NewGuid(), userId);
            bookedUsers = new Guid[1];
            bookedUsers[0] = userId;
            userCount = 1;
            initialBookingCount = session.GetCurrentBookingCount();
        }

        [Given(@"a session ""([^""]*)"" started (\d+) hour ago")]
        public void GivenASessionStartedHourAgo(string title, int hours)
        {
            var startTime = DateTime.Now.AddHours(-hours);
            var endTime = startTime.AddHours(2); // Ensure end time is after start time
            
            session = new Session(
                Guid.NewGuid(),
                title,
                $"Description for {title}",
                50,
                startTime,
                endTime,
                "Main Hall"
            );
        }

        [Given(@"the session is in the future")]
        public void GivenTheSessionIsInTheFuture()
        {
            // Already handled in the first Given clause
            session.StartTime.ShouldBeGreaterThan(DateTime.Now);
        }

        [Given(@"the session is not yet full after a cancellation")]
        public void GivenTheSessionIsNotYetFullAfterACancellation()
        {
            // We'll simulate a cancellation by cancelling one booking
            if (session.Bookings.Any())
            {
                var firstBooking = session.Bookings.First();
                session.CancelBooking(firstBooking.Id);
            }

            // For this scenario, "increase by X" should be measured from post-cancellation state.
            initialBookingCount = session.GetCurrentBookingCount();
        }

        [When(@"an (\d+)(?:st|nd|rd|th) user attempts to book the session")]
        public void WhenAnNthUserAttemptsToBookTheSession(int nth)
        {
            var userId = Guid.NewGuid();
            try
            {
                session.CreateBooking(Guid.NewGuid(), userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }
        }

        [When(@"a user attempts to book the session")]
        public void WhenAUserAttemptsToBookTheSession()
        {
            var userId = Guid.NewGuid();
            try
            {
                session.CreateBooking(Guid.NewGuid(), userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }
        }

        [When(@"a user books the session")]
        public void WhenAUserBooksTheSession()
        {
            newUserId = Guid.NewGuid();
            session.CreateBooking(Guid.NewGuid(), newUserId);
        }

        [When(@"the same user attempts to book the same session again")]
        public void WhenTheSameUserAttemptsToBookTheSameSessionAgain()
        {
            try
            {
                // Use the first booked user to attempt to book again
                session.CreateBooking(Guid.NewGuid(), bookedUsers[0]);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }
        }

        [When(@"a new user attempts to book the session")]
        public void WhenANewUserAttemptsToBookTheSession()
        {
            var userId = Guid.NewGuid();
            try
            {
                session.CreateBooking(Guid.NewGuid(), userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }
        }

        [Then(@"the system should throw a ""([^""]*)"" exception")]
        public void ThenTheSystemShouldThrowAnException(string exceptionType)
        {
            caughtException.ShouldNotBeNull($"Expected {exceptionType} exception but none was thrown");
            
            // Get the actual exception that was thrown by unwrapping TargetInvocationException
            // which is used when invoking methods through reflection
            Exception actualException = caughtException;
            while (actualException is System.Reflection.TargetInvocationException && actualException.InnerException != null)
            {
                actualException = actualException.InnerException;
            }
            
            // Now check the actual exception type and message
            actualException.ShouldBeOfType<Volo.Abp.BusinessException>($"Expected BusinessException but got {actualException.GetType().Name}");
            
            switch(exceptionType)
            {
                case "SessionFull":
                    actualException.Message.ToLower().ShouldContain("maximum capacity");
                    break;
                case "SessionAlreadyStarted":
                    actualException.Message.ToLower().ShouldContain("after it has started");
                    break;
                case "DuplicateBooking":
                    actualException.Message.ToLower().ShouldContain("already booked");
                    break;
                default:
                    Assert.True(false, $"Unknown exception type: {exceptionType}");
                    break;
            }
        }

        [Then(@"the booking count should remain (\d+)")]
        public void ThenTheBookingCountShouldRemain(int expectedCount)
        {
            session.GetCurrentBookingCount().ShouldBe(expectedCount);
        }

        [Then(@"the booking count should remain the same")]
        public void ThenTheBookingCountShouldRemainTheSame()
        {
            session.GetCurrentBookingCount().ShouldBe(initialBookingCount);
        }

        [Then(@"the booking should be confirmed")]
        public void ThenTheBookingShouldBeConfirmed()
        {
            session.GetCurrentBookingCount().ShouldBeGreaterThan(initialBookingCount);
            // The latest booking should be confirmed
            var latestBooking = session.Bookings.LastOrDefault();
            latestBooking.ShouldNotBeNull();
            latestBooking.Status.ShouldBe("Confirmed");
        }

        [Then(@"the session booked count should increase by (\d+)")]
        public void ThenTheSessionBookedCountShouldIncreaseBy(int increment)
        {
            session.GetCurrentBookingCount().ShouldBe(initialBookingCount + increment);
        }
    }
}
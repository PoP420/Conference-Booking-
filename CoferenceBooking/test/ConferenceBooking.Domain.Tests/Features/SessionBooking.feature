Feature: Session Booking
    As a conference attendee
    I want to book sessions
    So that I can attend the sessions I'm interested in

Scenario: Attempting to book a full session
    Given a session "DDD Workshop" with capacity 10
    And 10 users have already booked the session
    When an 11th user attempts to book the session
    Then the system should throw a "SessionFull" exception
    And the booking count should remain 10

Scenario: Attempting to book a past session
    Given a session "Keynote" started 1 hour ago
    When a user attempts to book the session
    Then the system should throw a "SessionAlreadyStarted" exception

Scenario: User successfully books a seat
    Given a session "Clean Code" with capacity 50
    And the session is in the future
    When a user books the session
    Then the booking should be confirmed
    And the session booked count should increase by 1

Scenario: User attempting to book the same session twice
    Given a session "Architecture Patterns" with capacity 20
    And a user has already booked this session
    When the same user attempts to book the same session again
    Then the system should throw a "DuplicateBooking" exception
    And the booking count should remain the same

Scenario: Booking becomes possible after another booking is cancelled
    Given a session "DDD Workshop" with capacity 2
    And 2 users have already booked the session
    And the session is not yet full after a cancellation
    When a new user attempts to book the session
    Then the booking should be confirmed
    And the session booked count should increase by 1
# 📘 Phase 1: Strategic Design & Technical Foundation

**Project:** ConferenceBooking  
**Phase:** 1 (Planning & Setup)  
**Objective:** Define the Domain Model, System Functionality, and Technical Foundation before writing implementation code.  
**Audience:** Development Team & AI Agent (Ollama + Qwen Code)

---

## 1. Domain Model (Strategic Design)

This section defines the **Bounded Context** and the **Aggregate Structure**. The AI agent must adhere to this model when generating code in subsequent phases.

### 1.1 Bounded Context
**Name:** Conference Booking Context  
**Responsibility:** Managing conference sessions and user bookings.  
**Integration:** Uses ABP Identity Module for User management (No need to create a custom User entity).

### 1.2 Aggregates & Entities

| Entity | Type | Description | Key Properties |
| :--- | :--- | :--- | :--- |
| **Session** | `AggregateRoot` | Represents a specific talk or workshop. | `Id`, `Title`, `Description`, `MaxCapacity`, `StartTime`, `EndTime`, `Location` |
| **Booking** | `Entity` (Child) | Represents a user's reservation within a Session. | `Id`, `SessionId`, `UserId`, `BookingTime`, `Status` |
| **User** | `Identity` | Managed by ABP Identity Module. | `Id`, `UserName`, `Email` (Referenced by `Booking.UserId`) |

### 1.3 Relationships
*   **Session 1 ── * Booking**: A Session has many Bookings.
*   **Booking * ── 1 User**: A Booking belongs to one User (ABP Identity).
*   **Navigation:** `Session` should hold a collection of `Bookings`. `Booking` should hold `UserId`.

### 1.4 Business Invariants (Rules)
*These rules must be enforced inside the Domain Layer.*

1.  **Capacity Constraint:** `Session.Bookings.Count` cannot exceed `Session.MaxCapacity`.
2.  **Time Constraint:** Booking cannot occur if `DateTime.UtcNow > Session.StartTime`.
3.  **Duplicate Constraint:** A `User` cannot have more than one `Booking` per `Session`.
4.  **Status Constraint:** A `Booking` can be `Confirmed` or `Cancelled`. Cancelled bookings do not count towards capacity.

### 1.5 Value Objects (Optional for MVP)
*   **TimeSlot:** `StartTime` + `EndTime`. (For MVP, use primitive `DateTime` to reduce complexity, but design as if it were a VO).
*   **Location:** `RoomName` + `Building`. (For MVP, use `string Location`).

---

## 2. System Functionality (Use Cases)

This section defines **what** the system does. The AI agent should use this to generate Application Services and DTOs.

### 2.1 Actors
1.  **Guest:** Unauthenticated user.
2.  **Attendee:** Authenticated user (can book).
3.  **Admin:** Authenticated user with role `admin` (can create sessions).

### 2.2 Use Case List

| ID | Use Case | Actor | Description | Input | Output |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **UC-01** | **View Sessions** | Guest | List all available sessions. | `FilterDto` (Optional) | `List<SessionDto>` |
| **UC-02** | **View Session Details** | Guest | View details of a specific session. | `SessionId` | `SessionDetailDto` |
| **UC-03** | **Book Session** | Attendee | Reserve a seat in a session. | `SessionId` | `BookingDto` |
| **UC-04** | **Cancel Booking** | Attendee | Cancel an existing reservation. | `BookingId` or `SessionId` | `Success` |
| **UC-05** | **View My Bookings** | Attendee | List sessions booked by current user. | None | `List<BookingDto>` |
| **UC-06** | **Create Session** | Admin | Add a new session to the system. | `CreateSessionDto` | `SessionDto` |

---

## 3. BDD Scenarios (High-Level)

These scenarios guide the **Test-First** approach. The AI agent should convert these into specific xUnit tests.

### Scenario: Booking a Full Session
```gherkin
Feature: Session Booking
  Scenario: Attempting to book a full session
    Given a session "DDD Workshop" with capacity 10
    And 10 users have already booked the session
    When an 11th user attempts to book the session
    Then the system should throw a "SessionFull" exception
    And the booking count should remain 10
```

### Scenario: Booking a Past Session
```gherkin
  Scenario: Attempting to book a past session
    Given a session "Keynote" started 1 hour ago
    When a user attempts to book the session
    Then the system should throw a "SessionAlreadyStarted" exception
```

### Scenario: Successful Booking
```gherkin
  Scenario: User successfully books a seat
    Given a session "Clean Code" with capacity 50
    And the session is in the future
    When a user books the session
    Then the booking should be confirmed
    And the session booked count should increase by 1
```

---

## 4. Technical Foundation (Setup Tasks)

*These tasks must be completed before Domain Coding begins.*

### 4.1 Solution Structure
Ensure the solution matches the ABP Layered Architecture:
```text
/ConferenceBooking.sln
├── /src
│   ├── /ConferenceBooking.Domain
│   ├── /ConferenceBooking.Application
│   ├── /ConferenceBooking.EntityFrameworkCore
│   ├── /ConferenceBooking.HttpApi
│   └── /ConferenceBooking.Blazor
├── /test
│   ├── /ConferenceBooking.Domain.Tests
│   └── /ConferenceBooking.Application.Tests
└── /database
    └── ConferenceBooking.db (Generated)
```

### 4.2 Database Configuration
*   **Provider:** SQLite
*   **Connection String:** `Data Source=ConferenceBooking.db`
*   **Migration Strategy:** Code First Migrations via `ConferenceBooking.DbMigrations` project.

### 4.3 CLI Commands Checklist
- [ ] `abp new ConferenceBooking -u blazor -d EF --dbms SQLite --pwa`
- [ ] Update `appsettings.json` with SQLite connection string.
- [ ] Run `dotnet run` on `ConferenceBooking.DbMigrations` to seed DB.
- [ ] Verify `dotnet test` runs successfully (empty tests).

---

## 5. AI Agent Instructions for Phase 1

*Specific instructions for the Ollama + Qwen Code agent during this phase.*

1.  **Model Validation:** When generating Entity classes, verify they match the **Section 1.2** table. Do not add extra properties without justification.
2.  **Invariant Enforcement:** When generating `Session.cs`, ensure methods like `Book()` check the rules in **Section 1.4**.
3.  **Test Generation:** When creating tests, base them on the **Section 3** Gherkin scenarios.
4.  **Identity Integration:** Do not create a `User` entity. Use `Volo.Abp.Identity.IdentityUser` via `CurrentUser` service.
5.  **DTO Separation:** When planning Application Layer, ensure DTOs (Section 2.2) do not mirror Entities exactly (e.g., exclude `Booking` collection from `SessionDto` to prevent circular refs).
6.  **Stop Condition:** Do not proceed to Phase 2 (Coding) until this Plan is confirmed by the user.

---

## 6. Deliverables for Phase 1 Completion

By the end of this phase, the following artifacts must exist:
1.  [ ] **Solution File:** Compiled successfully.
2.  [ ] **Database:** SQLite file created and migrated.
3.  [ ] **Documentation:** This Markdown file saved in `/docs/PHASE_1_PLAN.md`.
4.  [ ] **Model Diagram:** (Optional) Visual representation of Session/Booking relationship.
5.  [ ] **Test Skeletons:** Empty test classes created in `/test` projects matching the Use Cases.

---

## 7. Next Steps (Transition to Phase 2)

Once this plan is approved:
1.  **Step 1:** Create `Session` Aggregate Root class in `Domain`.
2.  **Step 2:** Write `Session_Tests.cs` based on BDD Scenarios.
3.  **Step 3:** Implement Domain Logic to pass tests.

---
*End of Phase 1 Detailed Plan.*
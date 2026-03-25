# 📘 ConferenceBooking: DDD & BDD Implementation Plan

**Project:** ConferenceBooking  
**Framework:** ABP Framework (.NET)  
**Goal:** Learn Domain-Driven Design (DDD) principles using a Behavior-Driven Development (BDD) approach.  
**AI Context:** This document serves as the master roadmap for the local Ollama + Qwen Code agent to assist in development, ensuring consistency with ABP conventions and DDD rules.

---

## 1. Project Overview
We are building a **Conference Booking System** where users can book seats for specific sessions.  
**Core Business Rules (Invariants):**
1.  **Capacity:** A session cannot exceed its maximum seat capacity.
2.  **Timing:** Bookings cannot be made after the session start time.
3.  **Uniqueness:** A user cannot book the same session twice.
4.  **Cancellation:** Cancelling a booking frees up a seat immediately.

---

## 2. Technology Stack
| Component | Technology | Notes |
| :--- | :--- | :--- |
| **Framework** | ABP Framework (v8+) | Modular architecture, dependency injection, UOW |
| **Language** | C# (.NET 8/9) | |
| **Database** | SQLite | Local file-based DB for learning simplicity |
| **ORM** | Entity Framework Core | Configured via ABP DbContext |
| **UI** | Blazor Server | ABP Blazor UI |
| **Testing** | xUnit + Shouldly | BDD-style assertions |
| **CLI** | ABP CLI | For scaffolding and migrations |

---

## 3. Architecture & Layers (ABP Standard)
*Strict dependency rules must be enforced by the AI agent.*

1.  **`ConferenceBooking.Domain`**
    *   **Responsibility:** Business logic, state, invariants.
    *   **Dependencies:** None (Pure .NET).
    *   **Key Items:** Aggregates, Entities, Value Objects, Domain Services, Domain Events.
2.  **`ConferenceBooking.Application`**
    *   **Responsibility:** Use cases, orchestration, DTOs.
    *   **Dependencies:** `Domain`.
    *   **Key Items:** App Services, DTOs, AutoMapper profiles.
3.  **`ConferenceBooking.EntityFrameworkCore`**
    *   **Responsibility:** Data persistence.
    *   **Dependencies:** `Domain`, `Application`.
    *   **Key Items:** DbContext, Migrations, Repository Implementations (auto).
4.  **`ConferenceBooking.Blazor`**
    *   **Responsibility:** UI Presentation.
    *   **Dependencies:** `Application` (via API or direct ref).
5.  **`test/ConferenceBooking.Domain.Tests`**
    *   **Responsibility:** Unit tests for Domain logic (In-memory).
6.  **`test/ConferenceBooking.Application.Tests`**
    *   **Responsibility:** Integration tests for App Services (Test DB).

---

## 4. DDD Principles & Rules
*The AI must enforce these rules during code generation.*

*   **Rich Domain Model:** Entities must contain business logic. **NO Anemic Models** (public setters on critical properties are forbidden).
*   **Aggregate Roots:** `Session` is an Aggregate Root. `Booking` is a child entity. All changes to `Booking` must go through `Session`.
*   **Repositories:** Only inject `IRepository<AggregateRoot, Id>` in Application Services. Do not inject `DbContext`.
*   **Exceptions:** Use `Volo.Abp.BusinessException` for business rule violations (e.g., `SessionFullException`).
*   **DTOs:** Never expose Domain Entities to the UI. Always use DTOs in Application Services.
*   **Unit of Work:** Application Services are automatically wrapped in a Unit of Work. Do not manually save changes unless necessary.

---

## 5. BDD Strategy (Test-First)
*Development flow for every feature:*

1.  **Red:** Write a failing test in `Domain.Tests` describing the behavior (Given-When-Then).
2.  **Green:** Implement the minimum Domain logic to pass the test.
3.  **Refactor:** Clean up code while keeping tests green.
4.  **Integration:** Write an `Application.Tests` test to verify the use case end-to-end.
5.  **UI:** Implement Blazor components only after tests pass.

**Test Naming Convention:**  
`Should_{ExpectedBehavior}_When_{Condition}`  
*Example:* `Should_ThrowException_When_BookingFullSession`

---

## 6. Implementation Roadmap

### Phase 1: Setup & Foundation ✅
- [x] Initialize ABP Solution (`abp new ConferenceBooking ...`)
- [x] Configure SQLite Connection String
- [x] Run Initial Migrations (`dotnet run` on DbMigrations)
- [x] Verify Solution Builds & Runs
- [ ] **AI Task:** Verify project structure matches Section 3.

### Phase 2: Domain Layer (Session Aggregate) 🚧 *Current Focus*
- [ ] Create `Session` Aggregate Root (`Domain/Sessions/Session.cs`)
- [ ] Create `Booking` Child Entity (`Domain/Sessions/Booking.cs`)
- [ ] **BDD:** Write `Session_Tests.cs` (Capacity, Time, Duplicate rules)
- [ ] **BDD:** Implement logic in `Session.cs` to pass tests
- [ ] Create `SessionManager` Domain Service (if logic becomes complex)
- [ ] **AI Task:** Ensure no `public set;` on critical properties.

### Phase 3: Application Layer (Use Cases)
- [ ] Create DTOs (`CreateBookingDto`, `SessionDto`, `BookingDto`)
- [ ] Create `SessionAppService` (`Application/Sessions/SessionAppService.cs`)
- [ ] **BDD:** Write `SessionAppService_Tests.cs`
- [ ] Implement `BookAsync`, `CancelAsync`, `GetListAsync`
- [ ] Configure AutoMapper Profiles
- [ ] **AI Task:** Ensure AppService only orchestrates, no business rules.

### Phase 4: Infrastructure & Data
- [ ] Configure `ConferenceBookingDbContext` (OnModelCreating)
- [ ] Add EF Core Migrations (`Add-Migration Initial`)
- [ ] Update Database
- [ ] **AI Task:** Verify relationships (Session 1..* Booking).

### Phase 5: UI (Blazor)
- [ ] Create `Sessions` page (List view)
- [ ] Create `SessionDetail` page (Book/Cancel actions)
- [ ] Handle BusinessException UI notifications (ABP MessageService)
- [ ] **AI Task:** Use ABP Blazor components (e.g., `<AbpTable>`, `<AbpButton>`).

### Phase 6: Advanced DDD (Optional)
- [ ] Implement Domain Events (`SessionBookedEvent`)
- [ ] Implement Event Handlers (e.g., Send Email on Booking)
- [ ] Implement Soft Delete for Cancellations

---

## 7. AI Agent Guidelines
*Instructions for the Ollama + Qwen Code Agent:*

1.  **Context Awareness:** Always check `Current Status` in this file before generating code.
2.  **ABP Conventions:**
    *   Use `IRepository<T, TKey>` instead of `DbSet<T>`.
    *   Use `CurrentUser.Id` for user identity (do not pass userId in DTOs unless necessary).
    *   Use `Async` methods everywhere.
    *   Inject services via Constructor Injection.
3.  **Code Generation:**
    *   When creating a new Entity, always include the `private` parameterless constructor for EF Core.
    *   When creating Tests, inherit from `ConferenceBookingDomainTestBase` or `ConferenceBookingApplicationTestBase`.
    *   Use `Shouldly` for assertions (e.g., `result.ShouldBeTrue()`).
4.  **Error Handling:**
    *   Never return `null` for business errors. Throw `BusinessException`.
    *   Do not catch `BusinessException` in the Domain layer.
5.  **Continuity:**
    *   If a file is modified, show the **full file content** or clear **diffs** to avoid context loss.
    *   Remind the user to run `dotnet test` after generating tests.

---

## 8. Current Status & Next Immediate Task

**Status:** Phase 1 (Setup) Complete. Phase 2 (Domain) Starting.  
**Last Action:** Corrected `abp new` command. Solution created.  
**Next Task:**
1.  Verify the solution opens correctly.
2.  Create the `Session` Aggregate Root class.
3.  Write the first BDD test (`Should_ThrowException_When_BookingFullSession`).

---

## 9. File Path Reference
*Keep these paths consistent.*

```text
/src
  /ConferenceBooking.Domain
    /Sessions
      Session.cs
      Booking.cs
      ISessionRepository.cs (Optional, usually inherited)
  /ConferenceBooking.Application
    /Sessions
      SessionAppService.cs
      /DTOs
        SessionDto.cs
        CreateBookingDto.cs
  /ConferenceBooking.EntityFrameworkCore
    /EntityFrameworkCore
      ConferenceBookingDbContext.cs
      Migrations/

/test
  /ConferenceBooking.Domain.Tests
    /Sessions
      Session_Tests.cs
  /ConferenceBooking.Application.Tests
    /Sessions
      SessionAppService_Tests.cs
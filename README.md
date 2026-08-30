# Parking Management System

A full-stack parking management application built with **Angular** for
the frontend and **ASP.NET Core Web API** for the backend.

## Current Features

The following functionality has been implemented and tested so far:

### Authentication

-   User login
-   User registration
-   JWT-based authentication
-   Authenticated API requests
-   User information stored locally for the frontend session
-   Logout functionality
-   Angular routing between authenticated and unauthenticated pages
-   CORS configuration between the Angular frontend and ASP.NET Core API

### Parking Spaces

-   Retrieve parking spaces from the backend
-   Display parking spaces in a responsive card/grid layout
-   Display parking-space number and vehicle type
-   Display parking-space status:
    -   Available
    -   Occupied
    -   Reserved
    -   Inactive
-   Book an available parking space
-   Individual booking loading state
-   Booking success and error messages
-   Handling of API `409 Conflict` responses
-   Refresh parking-space information after booking

### Bookings

-   Create a booking
-   Retrieve the current user's bookings
-   Display booking history
-   Display:
    -   Parking-space number
    -   Check-in time
    -   Check-out time
    -   Amount
    -   Booking status
-   Checkout a booking
-   Refresh the relevant data after checkout

### Subscriptions

-   Create a subscription for a parking space
-   Retrieve the current user's subscriptions
-   Cancel a subscription
-   Display subscription information
-   Individual subscription loading state
-   Subscription success and error handling

### Frontend Structure

The application has been separated into different pages/components for
the main user operations:

``` text
Dashboard
Parking Spaces
My Bookings
My Subscriptions
```

The parking-space, booking, and subscription functionality has separate
Angular services and models.

## Technology Stack

### Frontend

-   Angular
-   TypeScript
-   RxJS
-   Angular Router
-   Angular HttpClient
-   HTML/CSS

### Backend

-   ASP.NET Core Web API
-   Entity Framework Core
-   C#
-   JWT authentication
-   DTO-based API responses
-   Service/controller architecture

## Project Structure

The project currently follows a structure similar to:

``` text
Parking Management
│
├── Client / Angular
│   ├── components
│   │   ├── login
│   │   ├── register
│   │   ├── dashboard
│   │   ├── parking-spaces
│   │   ├── bookings
│   │   └── subscriptions
│   │
│   ├── models
│   │   ├── parking-space.model.ts
│   │   ├── booking.model.ts
│   │   └── subscription.model.ts
│   │
│   └── service
│       ├── parking-space.service.ts
│       ├── booking.service.ts
│       └── subscription.service.ts
│
└── Server / ASP.NET Core
    ├── Controllers
    │   ├── AuthController
    │   ├── ParkingSpacesController
    │   ├── BookingController
    │   └── SubscriptionController
    │
    ├── Services
    │   ├── Authentication services
    │   ├── ParkingSpaceService
    │   ├── BookingService
    │   └── SubscriptionService
    │
    ├── DTOs
    ├── Models
    └── Data
        └── ParkingManagementDbContext
```

## Setup and Run

### Prerequisites

Install:

-   .NET SDK compatible with the backend project
-   Node.js
-   Angular CLI
-   A database configured for the ASP.NET Core application

Verify the installations:

``` bash
dotnet --version
node --version
npm --version
ng version
```

### 1. Start the ASP.NET Core API

Open a terminal in the backend project directory:

``` bash
dotnet restore
dotnet build
dotnet run
```

The API is currently configured to run over HTTPS. During development,
the Angular application has been calling the API using:

``` text
https://localhost:7295
```

If the backend starts on a different port, update the Angular service
API URLs accordingly.

### 2. Start the Angular application

Open another terminal in the Angular project directory:

``` bash
npm install
ng serve
```

Then open the Angular development URL shown by the CLI, normally:

``` text
http://localhost:4200
```

### 3. Database

The backend uses Entity Framework Core through:

``` text
ParkingManagementDbContext
```

Make sure the database connection configured by the project is available
before starting the API.

If the project uses EF Core migrations, apply the existing migrations
using the project's configured migration workflow before running the
application.

## API Endpoints Implemented

The following API operations are currently used by the Angular
application.

### Authentication

``` text
POST /api/Auth/register
POST /api/Auth/login
```

### Parking Spaces

``` text
GET    /api/ParkingSpaces
GET    /api/ParkingSpaces/{id}
POST   /api/ParkingSpaces
DELETE /api/ParkingSpaces/{id}
```

### Bookings

``` text
POST /api/Booking
GET  /api/Booking/my
POST /api/Booking/{id}/checkout
```

### Subscriptions

``` text
POST /api/Subscription
GET  /api/Subscription/my
POST /api/Subscription/{id}/cancel
```

The exact API behavior and authorization rules are defined by the
current backend implementation.

## Important Development Notes

### CORS

The Angular application and ASP.NET Core API run on different origins
during development.

CORS was configured on the backend to allow the Angular application to
call the API directly.

### Authentication

Authenticated API controllers use ASP.NET Core authorization.

For example:

``` csharp
[Authorize]
public class ParkingSpacesController : ControllerBase
```

The Angular application sends the authentication token with protected
API requests.

### Parking Space Status

Parking-space status is represented separately from the basic `IsActive`
property.

The UI currently supports:

``` text
Available
Occupied
Reserved
Inactive
```

This allows a parking space to remain active while still being
unavailable because of a booking or reservation.

### Booking Errors

The frontend handles HTTP `409 Conflict` responses from booking
operations and displays the returned error message to the user.

### Loading States

Loading state is tracked per operation where appropriate.

For example, clicking one parking-space booking button should not make
every booking button display `Booking...`.

## Assumptions

The following are assumptions based on the current implementation:

1.  The backend is running locally over HTTPS.
2.  The development API URL currently used by Angular is
    `https://localhost:7295`.
3.  Authentication is JWT-based.
4.  Protected APIs require an authenticated user.
5.  The current user information is stored in browser `localStorage`.
6.  The database is managed by the ASP.NET Core application through
    Entity Framework Core.
7.  Booking and subscription business rules are enforced by the backend
    rather than trusted solely to the Angular UI.
8.  The current implementation is primarily focused on the user-facing
    parking workflow.
9.  An administrator module has **not yet been implemented**. It is
    planned as the next major feature.

## What I Would Improve With More Time

### 1. Admin Module

Build a dedicated administrator area with:

-   Admin authentication/authorization
-   Admin dashboard
-   Parking-space management
-   User management
-   Booking management
-   Subscription management
-   Revenue/statistics

Backend authorization should use an admin role rather than relying only
on Angular route protection.

### 2. Stronger Authentication

Improve authentication with:

-   Proper role/claim management
-   Angular route guards
-   HTTP interceptor for JWT tokens
-   Better token expiration handling
-   Refresh-token support if required
-   More secure handling of authentication state

### 3. Better UI/UX

Improve the visual design with:

-   Consistent component styling
-   Responsive mobile layout
-   Navigation/sidebar
-   Confirmation dialogs
-   Toast notifications
-   Better empty/loading/error states
-   Improved booking and subscription cards

### 4. Booking and Subscription Rules

Add and enforce more detailed business rules, such as:

-   Preventing conflicting bookings
-   Preventing duplicate active subscriptions
-   Subscription expiry handling
-   Clear distinction between reserved and occupied spaces
-   Proper availability calculation
-   Better concurrency handling when two users attempt to book the same
    space

### 5. Validation

Add stronger validation on both frontend and backend:

-   DTO validation
-   Required-field validation
-   Invalid GUID handling
-   Duplicate parking-space validation
-   Business-rule validation
-   Consistent API error responses

### 6. Testing

Add automated tests:

``` text
Backend
├── Unit tests
└── Integration/API tests

Frontend
├── Service tests
├── Component tests
└── Routing/authentication tests
```

Important scenarios to test include successful booking, duplicate
booking, checkout, subscription cancellation, unauthorized access, and
`409 Conflict` responses.

### 7. Production Configuration

Before production deployment:

-   Move environment-specific API URLs into Angular
    environments/configuration
-   Remove development-only logging
-   Configure production CORS properly
-   Use secure secrets/configuration management
-   Configure HTTPS correctly
-   Add structured logging
-   Add centralized exception handling
-   Configure database migrations/deployment properly

## Current Status

The core **user parking workflow is functional**:

``` text
Register
   ↓
Login
   ↓
Dashboard
   ↓
Parking Spaces
   ├── Book
   └── Subscribe
   ↓
My Bookings / My Subscriptions
   ├── Checkout
   └── Cancel Subscription
```

The next planned major feature is the **Admin Module**.

# Hotel Service - Hotel Management System 

A full-stack hotel management system for booking reservations, processing payments, and managing user preferences.

## Features 
- **User Authentication** 
  JWT-based registration/login with roles (User/Admin)
- **Hotel Management**   
  Browse hotels with pagination, view details, favorite hotels
- **Reservation System**   
  Select dates, choose rooms, calculate pricing
- **Payment Processing** 
  Secure payment integration with card details
- **User Profiles** 
  Manage personal info, view booking history, favorites
- **Admin Dashboard** 
  Manage hotels, rooms, view all reservations (Coming Soon)

## Tech Stack 🛠️
**Backend**  
.NET Core • Entity Framework Core • PostgreSQL • JWT • Swagger

**Frontend**  
React • TypeScript • React Router • Axios • SweetAlert2


## Getting Started 

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 16+](https://nodejs.org/)
- [PostgreSQL 14+](https://www.postgresql.org/download/)

### Installation

#### Backend Setup
```bash
# Clone repository
git clone https://github.com/yourusername/hotel-service.git
cd hotel-service/HotelService.Api

# Configure database
# Update appsettings.Development.json with your PostgreSQL credentials (use appsettings.txt)
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=HotelDB;Username=postgres;Password=yourpassword"
  }
}

# Run migrations
dotnet ef database update

# Start server
dotnet run

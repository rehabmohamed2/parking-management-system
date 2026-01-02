# Parking Management System (PMS)

A comprehensive, enterprise-grade parking management platform built with microservices architecture, event-driven design, and modern web technologies.

## Overview

This repository contains a complete parking management ecosystem consisting of four major components:

- **PMS-Backend**: Microservices backend built with .NET 8.0
- **PMS_Frontend_Admin**: Administrative portal built with Angular 21
- **PMS_Frontend_Parker**: Customer booking portal built with Angular 21
- **ERP_Service**: External ERP integration service

## Key Features

- Hierarchical parking site management with geographic boundaries
- Real-time customer ticket booking system
- Automated invoice generation and ERP integration
- Event-driven microservices architecture with Apache Kafka
- Multi-language support (English & Arabic)
- Complete DevOps automation with Docker Swarm and Jenkins
- Comprehensive logging and monitoring with ELK Stack
- High availability with service replicas and health checks

## Architecture

### System Components

```
┌─────────────────────┐
│  Admin Portal       │ ──────┐
│  (Angular 21)       │       │
└─────────────────────┘       │
                              ▼
                    ┌──────────────────┐
                    │  Site Service    │
                    │  (Port 5003)     │
                    └────────┬─────────┘
                             │ Kafka Events
                             ▼
┌─────────────────────┐  ┌──────────────────┐
│  Parker Portal      │  │ Booking Service  │
│  (Angular 21)       │──│  (Port 5001)     │
└─────────────────────┘  └────────┬─────────┘
                                  │ Kafka Events
                                  ▼
                         ┌──────────────────┐
                         │ Invoice Service  │
                         │  (Port 5002)     │
                         └────────┬─────────┘
                                  │
                                  ▼
                         ┌──────────────────┐
                         │   ERP Service    │
                         │   (External)     │
                         └──────────────────┘
```

### Technology Stack

#### Backend
- .NET 8.0 (C#)
- ASP.NET Core Web API
- Entity Framework Core 8.0.22
- Apache Kafka 7.6.0 (Event-driven messaging)
- FluentValidation
- Serilog (Structured logging)
- SQL Server 2022

#### Frontend
- Angular 21.0 (Standalone Components)
- TypeScript 5.9.2
- RxJS 7.8.0
- SCSS
- @ngx-translate (Internationalization)
- Reactive Forms

#### Infrastructure
- Docker & Docker Swarm
- Jenkins (CI/CD)
- Elasticsearch 8.11.0 + Kibana + Filebeat (ELK Stack)
- Nginx Alpine
- Kafka UI

#### Cloud Services
- Cloudinary (Document storage)
- Azure OneDrive (Invoice storage)
- Docker Hub (Container registry)

## Components

### 1. PMS-Backend (Microservices)

Event-driven microservices architecture with clean architecture principles.

#### Microservices

**Site Service (Port 5003)**
- Manages hierarchical parking sites (buildings, malls, zones)
- Stores geographic polygon boundaries for parking areas
- Publishes `SiteCreatedEvent` to Kafka
- Database: `PMS_Site`

**Booking Service (Port 5001)**
- Handles customer parking ticket bookings
- Syncs site data via Kafka event consumption
- Calculates booking duration and pricing
- Publishes `BookingCreatedEvent` to Kafka
- Database: `PMS_Booking`

**Invoice Service (Port 5002)**
- Auto-generates invoices from booking events
- Creates HTML invoice documents
- Integrates with external ERP system
- Azure OneDrive document storage
- Database: `PMS_Invoice`

#### Key Features
- **Clean Architecture**: API → Application → Infrastructure → Domain
- **Event-Driven Communication**: Kafka-based async messaging
- **Saga Pattern**: Distributed transactions via event choreography
- **Repository Pattern**: Data access abstraction
- **Unit of Work Pattern**: Transaction management with event publishing
- **SharedKernel Libraries**: 10+ reusable infrastructure components

#### API Endpoints

**Site Service**
```
POST /api/site/add/parent     - Create parent site
POST /api/site/add/leaf       - Create parking zone with polygons
GET  /api/site/roots          - Get all root sites
GET  /api/site/leaves         - Get all parking zones
GET  /api/site/children/{id}  - Get child sites
```

**Booking Service**
```
POST /api/booking             - Create parking ticket
GET  /api/booking/leaves      - Get available parking sites
GET  /api/booking/health      - Health check
```

**Invoice Service**
```
POST /api/invoice             - Create invoice
GET  /api/invoice/health      - Health check
```

### 2. PMS_Frontend_Admin (Admin Portal)

Professional Angular-based administrative portal for managing parking infrastructure.

#### Features
- Hierarchical site management (parent/leaf structure)
- Geographic polygon boundary definition
- Interactive coordinate point management (lat/lng)
- Dual language support (English/Arabic)
- Tree navigation for site hierarchy
- Comprehensive form validation
- Professional UI/UX with responsive design

#### Key Components
- Admin Dashboard with tree navigation
- Site creation/editing forms
- Polygon management interface
- Site details display
- Language switcher
- Custom validators for geographic coordinates

#### Validation Rules
- Site names: 3-100 characters
- Latitude: -90 to +90 (6 decimal places)
- Longitude: -180 to +180 (6 decimal places)
- Minimum 3 polygon points per parking zone
- Price per hour: Numeric, 2 decimal places
- Parking slots: 1-10,000

### 3. PMS_Frontend_Parker (Customer Portal)

Customer-facing booking portal for parking reservations.

#### Features
- View available parking zones
- Real-time price calculation
- Plate number validation (Saudi format)
- Phone number validation
- Hour-based booking (1-24 hours)
- Ticket confirmation display
- Bilingual support with RTL for Arabic

#### Validation
- Plate Number: Saudi format (4 digits + 3 letters, e.g., "1234ABC")
- Phone Number: Saudi format (10 digits, starting with 05)
- Hours: 1-24 hours
- Site: Required selection

### 4. ERP_Service (Integration Service)

External ERP integration service with JWT authentication.

#### Features
- JWT-based authentication
- Invoice receipt and storage
- BCrypt password hashing
- Cloudinary document storage
- Ticket uniqueness validation
- Automatic tax calculation (10%)

#### API Endpoints
```
POST /Auth/login              - Login with email/password
POST /Invoice                 - Create invoice (requires JWT)
GET  /Invoice/all             - Get all invoices
```

## Deployment

### Docker Swarm Architecture

The system is deployed using Docker Swarm with:
- **2 replicas per service** for high availability
- **Overlay network** (`pms-network`) for service communication
- **Rolling updates** with zero downtime
- **Health checks** for all services
- **Resource limits**: 0.5 CPU / 256MB per service
- **Secrets management** for sensitive data

### Infrastructure Services
- Apache Kafka (port 9092) - Message broker
- Zookeeper (port 2181) - Kafka coordination
- SQL Server 2022 (port 1433) - 3 separate databases
- Elasticsearch (port 9200) - Log storage
- Kibana (port 5601) - Log visualization
- Filebeat - Log shipping (global deployment)
- Kafka UI (port 8090) - Kafka monitoring

### CI/CD Pipeline

7-stage Jenkins pipeline:
1. **Checkout** - Pull from GitHub
2. **Verify Docker Swarm** - Ensure Swarm is active
3. **Docker Login** - Authenticate to Docker Hub
4. **Build Images** - Build with version tags
5. **Deploy Stack** - Deploy to Swarm
6. **Health Check** - Wait 240s for services
7. **Run Migrations** - Initialize databases

## Event Flow

```
Admin creates site
    ↓
Site Service saves & publishes SiteCreatedEvent → Kafka
    ↓
Booking Service consumes event & stores site locally
    ↓
Customer creates booking via Parker Portal
    ↓
Booking Service creates ticket & publishes BookingCreatedEvent → Kafka
    ↓
Invoice Service consumes event & generates invoice
    ↓
Invoice Service sends to ERP Service
    ↓
ERP Service uploads HTML to Cloudinary & stores record
```

## Getting Started

### Prerequisites
- Docker & Docker Swarm
- .NET 8.0 SDK
- Node.js 20+
- SQL Server 2022
- Apache Kafka

### Backend Setup

```bash
cd PMS-Backend

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run migrations
./run-migrations.sh

# Deploy to Docker Swarm
docker stack deploy -c docker-compose.swarm.yml pms
```

### Frontend Setup

#### Admin Portal
```bash
cd PMS_Frontend_Admin

# Install dependencies
npm ci

# Development server
ng serve

# Production build
ng build

# Docker deployment
docker-compose up -d
```

#### Parker Portal
```bash
cd PMS_Frontend_Parker

# Install dependencies
npm ci

# Development server
ng serve

# Production build
ng build

# Docker deployment
docker-compose up -d
```

### ERP Service Setup

```bash
cd ERP_Service

# Restore dependencies
dotnet restore

# Run migrations
dotnet ef database update

# Run service
dotnet run

# Docker deployment
docker-compose up -d
```

## Configuration

### Environment Variables

#### Backend Services
```bash
# Connection Strings
ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PMS_*;...

# Kafka
Kafka__BootstrapServers=kafka:9092

# Azure OneDrive (Invoice Service)
CLIENT_ID=<Azure AD client ID>
CLIENT_SECRET=<Azure AD client secret>
TENANT_ID=<Azure AD tenant ID>
ONEDRIVE_FOLDER=<OneDrive folder path>
```

#### ERP Service
```bash
# JWT Settings
JwtSettings__Issuer=<issuer>
JwtSettings__Audience=<audience>
JwtSettings__SecretKey=<secret>

# Cloudinary
CLOUDINARY_CLOUD_NAME=<cloud name>
CLOUDINARY_API_KEY=<api key>
CLOUDINARY_API_SECRET=<api secret>
```

## Monitoring & Logging

### ELK Stack Integration
- **Filebeat**: Collects logs from all services
- **Elasticsearch**: Stores and indexes logs
- **Kibana**: Visualizes logs and creates dashboards

### Access Monitoring Tools
- Kibana: `http://localhost:5601`
- Kafka UI: `http://localhost:8090`
- SQL Server: `localhost:1433`

## Project Structure

```
D:\giza systems\for_github\
├── PMS-Backend/
│   ├── Booking.API/              # Booking microservice
│   ├── Invoice.API/              # Invoice microservice
│   ├── Site.API/                 # Site microservice
│   ├── SharedKernel/             # 10+ shared libraries
│   ├── docker-compose.swarm.yml  # Production deployment
│   ├── Jenkinsfile               # CI/CD pipeline
│   └── run-migrations.sh         # Database setup
│
├── PMS_Frontend_Admin/
│   ├── src/app/
│   │   ├── components/           # UI components
│   │   ├── models/               # TypeScript models
│   │   ├── services/             # API services
│   │   └── validators/           # Form validators
│   ├── src/assets/i18n/          # Translations
│   └── Dockerfile
│
├── PMS_Frontend_Parker/
│   ├── src/app/
│   │   ├── features/booking-form/  # Booking feature
│   │   ├── services/               # API & translation
│   │   └── shared/                 # Shared components
│   ├── Jenkinsfile
│   └── docker-compose.yml
│
└── ERP_Service/
    ├── Controllers/              # API endpoints
    ├── services/                 # Business logic
    ├── models/                   # Entities
    ├── DataBase/                 # EF Core context
    └── Migrations/               # Database migrations
```

## Design Patterns

- **Microservices Architecture**: Independent, scalable services
- **Event-Driven Design**: Kafka-based async communication
- **Clean Architecture**: Separation of concerns (API → Application → Infrastructure → Domain)
- **Repository Pattern**: Data access abstraction
- **Unit of Work Pattern**: Transaction management
- **Saga Pattern**: Distributed transaction choreography
- **CQRS**: Command/Query separation in services
- **Dependency Injection**: Service registration and IoC

## Best Practices

- **Security**: JWT authentication, BCrypt password hashing, input validation
- **Scalability**: Horizontal scaling with replicas, event-driven async processing
- **Reliability**: Health checks, retry mechanisms, graceful degradation
- **Maintainability**: Clean architecture, shared kernel libraries, comprehensive logging
- **Internationalization**: Multi-language support with i18n
- **Testing**: Unit tests, integration tests, Vitest for frontend
- **DevOps**: CI/CD automation, containerization, infrastructure as code

## API Documentation

Each service includes Swagger/OpenAPI documentation accessible at:
- Site Service: `http://localhost:5003/swagger`
- Booking Service: `http://localhost:5001/swagger`
- Invoice Service: `http://localhost:5002/swagger`
- ERP Service: `http://localhost:<port>/swagger`

## Database Schema

### Site Service (PMS_Site)
- **Sites**: Hierarchical site structure
- **Polygons**: Geographic boundaries
- **PolygonPoints**: Latitude/longitude coordinates

### Booking Service (PMS_Booking)
- **Tickets**: Parking bookings
- **Sites**: Replicated from Site Service

### Invoice Service (PMS_Invoice)
- **Invoices**: Generated invoices with HTML documents
- **Tickets**: Replicated from Booking Service

### ERP Service
- **Invoices**: External ERP invoice records
- **Users**: Authentication users

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please open an issue in the GitHub repository.

## Acknowledgments

- Built with .NET 8.0 and Angular 21
- Event-driven architecture powered by Apache Kafka
- Logging infrastructure provided by ELK Stack
- Containerization and orchestration with Docker Swarm
- CI/CD automation with Jenkins

---

**Built with modern technologies and enterprise-grade architecture for scalable parking management.**

# Technical Requirements Document (TRD)

| **Field** | **Value** |
| --- | --- |
| Project Name | Zakerly |
| Version | 1.0 |
| Status | Draft |
| Authors | Benho Jr |
| Date | July 2026 |

# **1. Document Overview**

## **Purpose**

This document defines the technical requirements for implementing the Zakerly Learning Management System (LMS). It specifies the technologies, software architecture, development standards, infrastructure requirements, security requirements, database requirements, and technical constraints necessary to successfully build and deploy the system.

## **Scope**

This document covers all technical aspects of the backend implementation, including application architecture, APIs, database, authentication, deployment environment, and development standards.

# **2. System Overview**

### **Overview**

The Zakerly Learning Management System is implemented as a web-based RESTful application using [ASP.NET](http://asp.net/) Core. The system follows a hybrid architecture combining Clean Architecture and Vertical Slice Architecture, providing a scalable, maintainable, and modular backend. PostgreSQL serves as the primary database, while JWT Bearer Authentication secures protected resources.

| **Component** | **Technology** |
| --- | --- |
| Backend | ASP.NET Core Web API |
| Database | PostgreSQL |
| Authentication | JWT |
| Architecture | Clean + Vertical Slice |
| Deployment | Docker |

# **3. Development Environment**

| **Component** | **Requirement** |
| --- | --- |
| IDE | JetBrains Rider |
| SDK | .NET 9 SDK |
| Database | PostgreSQL 17 |
| Version Control | Git |
| API Testing | Postman |
| Documentation | Swagger |
| Containerization | Docker Desktop |

# **4. Technology Requirements**

| **Component** | **Technology** |
| --- | --- |
| Language | C# |
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core |
| Database | PostgreSQL |
| Validation | FluentValidation |
| Authentication | JWT |
| Mapping | Mapster |
| Logging | ASP.NET Logging |
| API Docs | Swagger |
| Mediation | MediatR |

# **5. Hardware Requirements**

## **Development Machine**

| **Component** | **Minimum** |
| --- | --- |
| CPU | Dual Core |
| RAM | 8 GB |
| Storage | 20 GB Free |
| OS | Windows / Linux / macOS |

## **Server Requirements**

| **Component** | **Minimum** |
| --- | --- |
| CPU | 2 vCPU |
| RAM | 4 GB |
| Storage | 50 GB SSD |
| Database | PostgreSQL |

# **6. Software Requirements**

| **Software** | **Version** |
| --- | --- |
| .NET SDK | 9 |
| PostgreSQL | 17 |
| Docker | Latest |
| Git | Latest |

# **7. Database Requirements**

| **Requirement** | **Description** |
| --- | --- |
| Database Engine | PostgreSQL |
| Primary Keys | UUID |
| Foreign Keys | Required |
| Migrations | Entity Framework Core |
| Audit Fields | CreatedAt, UpdatedAt, CreatedBy, UpdatedBy |
| Soft Delete | Supported |

# **8. API Requirements**

| **Requirement** | **Description** |
| --- | --- |
| RESTful APIs | Yes |
| JSON Format | Yes |
| Versioning | /api/v1 |
| JWT Authentication | Required |
| Swagger Documentation | Required |

# **9. Authentication Requirements**

| **Requirement** | **Description** |
| --- | --- |
| Authentication | JWT Bearer |
| Password Hashing | BCrypt |
| Authorization | Role-Based |
| HTTPS | Required |

# **10. Security Requirements**

| **Requirement** | **Description** |
| --- | --- |
| HTTPS | Required |
| JWT | Required |
| Input Validation | FluentValidation |
| Authorization | Role Based |
| Password Hashing | BCrypt |
| Environment Variables | Secrets Storage |

# **11. Performance Requirements**

| **Requirement** | **Target** |
| --- | --- |
| API Response Time | < 2 Seconds |
| Database Connection | Pooled |
| Pagination | Required |
| Async Operations | Required |

# **12. Coding Standards**

- Follow SOLID Principles
- Use Dependency Injection
- Follow Clean Architecture
- Use Vertical Slice Architecture
- Meaningful Naming Conventions
- Async/Await
- Repository Pattern (if applicable)
- DTO Separation
- Validation Before Business Logic

# **13. Logging Requirements**

| **Requirement** | **Description** |
| --- | --- |
| Request Logging | Required |
| Error Logging | Required |
| Warning Logging | Required |
| Information Logging | Required |

# **14. Error Handling Requirements**

### **Overview**

The application uses centralized exception handling through Global Exception Middleware to ensure consistent error responses across all API endpoints.

| **Requirement** | **Description** |
| --- | --- |
| Global Exception Middleware | Handles unhandled exceptions centrally |
| Validation Errors | Returns HTTP 400 |
| Business Exceptions | Returns appropriate HTTP status codes |
| Standard Error Response | Consistent JSON error format |
| Logging | All exceptions are logged |

# **15. Deployment Requirements**

| **Component** | **Requirement** |
| --- | --- |
| Application | Docker Container |
| Database | PostgreSQL Container |
| Configuration | Environment Variables |
| Network | HTTPS |
| Reverse Proxy | Nginx (Optional) |

# **16. Testing Requirements**

| **Test Type** | **Required** |
| --- | --- |
| Unit Testing | Yes |
| Integration Testing | Yes |
| API Testing | Yes |
| Manual Testing | Yes |

# **17. Constraints**

- PostgreSQL must be used.
- [ASP.NET](http://asp.net/) Core 9 is required.
- JWT authentication is mandatory.
- Clean Architecture must be followed.
- Vertical Slice Architecture must be followed.

# **18. Assumptions**

- Internet connection is available.
- PostgreSQL server is running.
- Docker is installed.
- Users access the system through modern web browsers.

# **19. Future Technical Improvements**

- Redis Caching
- SignalR
- Background Jobs (Hangfire)
- Email Service
- Refresh Tokens
- Rate Limiting
- Health Checks
- Monitoring (Prometheus/Grafana)
- CI/CD Pipeline
- Cloud Deployment (Azure/AWS)
- Distributed Caching
- Centralized Logging

# **20. Configuration Requirements**

### **Overview**

The application configuration is managed using [ASP.NET](http://asp.net/) Core configuration providers and environment variables. Sensitive values are stored outside the source code to improve security and deployment flexibility.

| **Configuration** | **Description** |
| --- | --- |
| Connection String | PostgreSQL database connection |
| JWT Secret | Secret key used for token signing |
| JWT Expiration | Token lifetime configuration |
| Logging Level | Configures application logging |
| Environment | Development / Staging / Production |

# **21. Dependency Management**

### **Overview**

The project relies on NuGet packages to manage external dependencies. All packages should remain compatible with the target .NET version and be updated regularly to receive security patches and performance improvements.

| **Package** | **Purpose** |
| --- | --- |
| Entity Framework Core | ORM |
| MediatR | CQRS |
| FluentValidation | Validation |
| Mapster | Object Mapping |
| BCrypt.Net | Password Hashing |
| Swashbuckle | Swagger |
| Npgsql | PostgreSQL Provider |

# **22. API Versioning Strategy**

### **Overview**

To maintain backward compatibility, all APIs are versioned using URL versioning

Future API versions should be introduced without breaking existing clients.

| **Version** | **Base URL** |
| --- | --- |
| Version 1 | `/api/v1` |

# **23. Monitoring & Diagnostics**

### **Overview**

The application should provide sufficient monitoring and diagnostic information to simplify troubleshooting and performance analysis.

| **Requirement** | **Description** |
| --- | --- |
| Request Logging | Log incoming requests |
| Exception Logging | Log unhandled exceptions |
| Health Checks | Verify application availability |
| Performance Monitoring | Monitor response times |
| Database Connectivity | Verify database status |

# **24. Technical Risks**

### **Overview**

Potential technical risks should be identified early to reduce implementation and deployment issues.

| **Risk** | **Mitigation** |
| --- | --- |
| Database Failure | Regular backups and connection retry policies |
| Invalid Input | FluentValidation |
| Unauthorized Access | JWT Authentication & RBAC |
| Data Loss | Database transactions and backups |
| Dependency Updates | Regular package maintenance |

**Technical Requirements Document (TRD)**
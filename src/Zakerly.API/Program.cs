using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Zakerly.Application;
using Zakerly.Infrastructure;
using Zakerly.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

// Validate JWT configuration at startup
var jwtKey = builder.Configuration["JwtSettings:Key"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];
var jwtExpiryString = builder.Configuration["JwtSettings:ExpiryInMinutes"];

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT configuration error: 'JwtSettings:Key' is missing or empty.");
}

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
if (keyBytes.Length < 32)
{
    throw new InvalidOperationException("JWT configuration error: 'JwtSettings:Key' must be at least 32 bytes (256 bits) for HS256.");
}

if (string.IsNullOrEmpty(jwtIssuer))
{
    throw new InvalidOperationException("JWT configuration error: 'JwtSettings:Issuer' is missing or empty.");
}

if (string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("JWT configuration error: 'JwtSettings:Audience' is missing or empty.");
}

if (!int.TryParse(jwtExpiryString, out var jwtExpiry) || jwtExpiry <= 0)
{
    throw new InvalidOperationException("JWT configuration error: 'JwtSettings:ExpiryInMinutes' must be a valid positive integer.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
using erp_api.Data;
using erp_api.Repositories;
using erp_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Database Configuration
// -----------------------------
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------------
// Dependency Injection (DI)
// -----------------------------
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Repository
builder.Services.AddScoped<IEmployeeService, EmployeeService>();       // Service Layer
builder.Services.AddScoped<JwtService>();                              // JWT Helper Service
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// -----------------------------
// JSON Serializer Options
// (Keeps PascalCase in JSON instead of camelCase)
// -----------------------------
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// -----------------------------
// Swagger / OpenAPI Config
// -----------------------------
builder.Services.AddOpenApi();              // Adds minimal OpenAPI support (for .NET 8/9)
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger UI generation

builder.Services.AddSwaggerGen(options =>
{
    // Basic info about the API
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP API", Version = "v1" });

    // JWT Bearer Token support in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",                      // Header name
        Type = SecuritySchemeType.Http,              // Use HTTP Auth
        Scheme = "Bearer",                           // Scheme type
        BearerFormat = "JWT",                        // Format of the token
        In = ParameterLocation.Header,               // Token is passed in the header
        Description = "Paste your JWT token below (no need to type 'Bearer')."
    });

    // Apply the Bearer token to all endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// -----------------------------
// JWT Authentication Config
// -----------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,           // Validate the issuer (e.g., your domain)
            ValidateAudience = true,         // Validate the audience (e.g., frontend app)
            ValidateLifetime = true,         // Validate token expiry
            ValidateIssuerSigningKey = true, // Validate the secret key used to sign token

            // Values from appsettings.json (e.g., Jwt:Issuer, Jwt:Key, etc.)
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// -----------------------------
// Add CORS Support
// -----------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200","http://127.0.0.1:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

// -----------------------------
// HTTP Pipeline Configuration
// -----------------------------
if (app.Environment.IsDevelopment())
{
    // Enables Swagger UI only in Development
    app.MapOpenApi(); // Serves OpenAPI at /openapi.json (for .NET 8+)
    app.UseSwagger(); // Serves /swagger/v1/swagger.json
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API V1");
        c.RoutePrefix = "swagger"; // Swagger UI at /swagger
    });
}

app.UseHttpsRedirection();     // Redirects HTTP requests to HTTPS

app.UseCors("CustomPolicy");   // Use CORS Middleware in HTTP Pipeline - After app.Build(), before authentication

app.UseAuthentication();       // Enables JWT Authentication middleware
app.UseAuthorization();        // Enables Role/Policy-based Authorization

app.MapControllers();          // Maps all controller routes (API endpoints)

app.MapGet("/", () => "Welcome to My .NET 9 erp-api!"); // Root test endpoint

app.Run();                     // Runs the application

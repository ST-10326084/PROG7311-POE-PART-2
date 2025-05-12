using Core.Data;
using Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// @see https://learn.microsoft.com/en-us/aspnet/core/fundamentals/startup
// @see https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration
// @see https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt

var builder = WebApplication.CreateBuilder(args);

// Register the database context using SQLite and the configured connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AuthService for dependency injection
builder.Services.AddScoped<AuthService>();

// Enable session-based state storage
builder.Services.AddSession();

// Add support for MVC controllers with views (Razor)
builder.Services.AddControllersWithViews();

// Configure JWT-based authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Enable role-based authorization
builder.Services.AddAuthorization();

// Enable Swagger for API testing (dev only)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add middleware components
app.UseRouting();
app.UseStaticFiles();      // Serves static files from wwwroot
app.UseSession();          // Enables session management
app.UseAuthentication();   // Enables JWT authentication
app.UseAuthorization();    // Enables role-based access control

// Maps default MVC controller route: /Controller/Action/Id
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed the database on application startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Core.DataSeeder.Seed(context);
}

// Run the web application
app.Run();

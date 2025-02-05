using System.Text;
using mediumBE.Data;
using mediumBE.Endpoints;
using mediumBE.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add JWT Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not found")))
        };
    });

builder.Services.AddAuthorization();

// Register JwtService
builder.Services.AddScoped<JwtService>();

var connectionString = builder.Configuration.GetConnectionString("SqliteDb");
builder.Services.AddSqlite<MediumContext>(connectionString);

var app = builder.Build();

// Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
    
app.MapAuthEndpoints();
// Add the new article endpoints
app.MapArticleEndpoints();
app.MapCommentEndpoints();
app.MapProfileEndpoints();

app.Run();

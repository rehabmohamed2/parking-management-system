using System.Text;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using IntegrationServices.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
       {
           options.AddPolicy("AllowAll",
               policy =>
               {
                   policy
                       .AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
               });
       });

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=db36281.public.databaseasp.net; Database=db36281; User Id=db36281; Password=6Zs%Lc?8w9H-; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;"));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Register application services
builder.Services.AddScoped<IntegrationServices.services.InvoiceServices>();
builder.Services.AddScoped<IntegrationServices.services.JwtService>();
builder.Services.AddScoped<IntegrationServices.services.CloudinaryUploader>();

var app = builder.Build();

// Ensure database is created at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

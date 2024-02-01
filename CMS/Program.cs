using CMS.Entities;
using CMS.Interfaces;
using CMS.Persistence;
using CMS.Persistence.Seed;
using CMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 1. DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("db")));

// Add support for roles
builder.Services.AddIdentity<User, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpClient();


// 3. Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})


// 4. Add Jwt Bearer
      .AddJwtBearer(options =>
      {
          options.SaveToken = true;
          options.RequireHttpsMetadata = false;
          options.TokenValidationParameters = new TokenValidationParameters()
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidAudience = configuration["JWT:validAudience"],
              ValidIssuer = configuration["JWT:validIssuer"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
          };
      });


// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ElevatedRights", policy =>
        policy.RequireRole(Role.Admin));
    options.AddPolicy("StandardRights", policy =>
        policy.RequireRole(Role.Admin, Role.User));
});


// Add services to the container.
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = " Church Management API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Berer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<String>()
            }
        });

});

// 6. Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDevClient",
        b =>
        {
            b
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// 7. Use CORS
app.UseCors("AllowDevClient");
app.UseHttpsRedirection();

// 8. use Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeedManager.Seed(services);
}

app.Run();

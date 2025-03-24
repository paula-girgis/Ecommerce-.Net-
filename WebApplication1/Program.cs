using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Api.Errors;
using WebApplication1.Api.Middlewares;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;
using WebApplication1.Repositry;
using WebApplication1.Repositry.Data;
using WebApplication1.Service;
using WebApplication1.Service.WebApplication1.Repositry.WebApplication1.Core.services;
using CloudinaryDotNet;
using Npgsql;
using Microsoft.Extensions.Configuration;
using WebApplication1.Repositry.Repositories;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Services Registration

        //CORS Policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });



        // Controllers and JSON
        builder.Services.AddControllers();
      

        // Database Context
        builder.Services.AddDbContext<EcommerceContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


        // Email Service
        builder.Services.AddScoped<IEmailService, EmailService>();

        // Token Services
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<TokenService>();


        // Generic Repository
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        

        // Identity Configuration
        builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => { options.Password.RequireNonAlphanumeric = false; })
            .AddEntityFrameworkStores<EcommerceContext>()
            .AddDefaultTokenProviders();

        // JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

        // Swagger Documentation
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Validation Error Handling
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                                          .Where(e => e.Value.Errors.Any())
                                          .SelectMany(e => e.Value.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = errors });
            };
        });

        builder.Services.AddHttpClient();

        #endregion

        var app = builder.Build();

        #region Middleware Pipeline

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<TokenAuthenticationMiddleware>();

        app.UseHttpsRedirection();

        app.UseCors("AllowFrontend");
        //app.UseCors("AllowReactApp");

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStatusCodePagesWithRedirects("/Redirect/{0}");
        app.MapControllers();

        #endregion

        #region Database Migration
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var dbContext = services.GetRequiredService<EcommerceContext>();
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration.");
            }
        }
        #endregion

        app.Run();
    }
}


using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Real_Estatae_Project.DTO.Cloudinary;
using Real_Estatae_Project.Hubs;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using Stripe;
using System.Security.Claims;

using System.Text;

namespace Real_Estatae_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ProjectContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });



            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ProjectContext>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
               options =>
               {
                   options.Password.RequireNonAlphanumeric = true;
                   options.Password.RequireLowercase = true;
                   options.Password.RequireUppercase = true;
                   options.Password.RequireDigit = true;
                   options.Password.RequiredLength = 8;
               })
               .AddEntityFrameworkStores<ProjectContext>();

            //services

            builder.Services.AddScoped<IUnitRepository, UnitRepository>();
            builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
            builder.Services.AddScoped<IReactRepository, ReactRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRentRepositories, RentRepositories>();

            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();

            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

            // SignalR
            builder.Services.AddSignalR().AddHubOptions<NotificationHub>(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
            });

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudienceIP"],

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),

                    NameClaimType = ClaimTypes.NameIdentifier
                };

                // SignalR support: accept token in query string
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notification"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FreePlan", policy =>
                {

                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });

            // Hangfire
            builder.Services.AddHangfire(config =>
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("cs")));
            builder.Services.AddHangfireServer();

            // Stripe
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
          
            //cloudinary
            builder.Services.Configure<CloudinarySettings>(
            builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();


            var app = builder.Build();

            // Swagger

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("FreePlan");

            app.UseHangfireDashboard();

            // Repeating jobs
            RecurringJob.AddOrUpdate<IRentRepositories>(
                "generate-monthly-rents",
                x => x.GenerateMonthlyRentsAsync(),
                Cron.Monthly);

            app.MapControllers();
            app.MapHub<NotificationHub>("/hubs/notification");

            app.Run();
        }
    }
}


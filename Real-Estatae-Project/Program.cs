
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Real_Estatae_Project.DTO.Cloudinary;
using Real_Estatae_Project.Hubs;
using Real_Estatae_Project.Repositories;
using Real_Estatae_Project.Services;
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
                   //to lock user for 15min, if they failed to login fo 5 times
                   options.Lockout.MaxFailedAccessAttempts = 5;
                   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                   options.Password.RequireNonAlphanumeric = true;
                   options.Password.RequireLowercase = true;
                   options.Password.RequireUppercase = true;
                   options.Password.RequireDigit = true;
                   options.Password.RequiredLength = 8;
               })
               .AddEntityFrameworkStores<ProjectContext>()
               .AddDefaultTokenProviders();

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
            builder.Services.AddScoped<IOpenAIService, OpenAIService>();
            builder.Services.AddScoped<IImportFile, ImportFile>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();


            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

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

            #region Cloudinary
            //cloudinary
            builder.Services.Configure<CloudinarySettings>(
            builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();

            //to encrease request size
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 104_857_600; // 100 MB
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);   // increase keep-alive
                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10); // increase headers timeout
            });

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 104_857_600;
                options.MaxRequestBodyBufferSize = 104_857_600;
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10_000_000; // 10MB or higher
            });

            //to encrease request timeout 
            #endregion


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
               // Cron.Monthly
               Cron.Minutely
            );

            app.MapControllers();
            app.MapHub<NotificationHub>("/hubs/notification");

            app.Run();
        }
    }
}

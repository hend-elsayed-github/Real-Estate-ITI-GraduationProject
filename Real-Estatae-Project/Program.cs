
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Real_Estate_Project.Models;
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


            builder.Services.AddAuthentication(options =>
            {
                //Search Bearer Keyword for ==>  JWT in Header 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //To Change from Route of Cookie ==> Account/Login
                //To JWT => UnAuth.
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                //For Any other behaviours ==> Auth. Filter search JWT 
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //How to check if Token Valid 
                options.SaveToken = true; //Not Expired

                options.RequireHttpsMetadata = false; //specific protocole 

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudienceIP"],

                    //Check Signature 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };


            });;


            //corse policy

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FreePlan", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });


            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                // Disable automatic model state validation
                options.SuppressModelStateInvalidFilter = true;

                // Disable automatic response for validation errors
                options.SuppressMapClientErrors = true;
            });



            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors("FreePlan");

            app.MapControllers();

            app.Run();
        }
    }
}

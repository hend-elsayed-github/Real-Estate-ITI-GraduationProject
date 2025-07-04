
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estate_Project.Models;

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

            //builder.Services.AddAuthentication(options =>
            //{
            //    //Search Bearer Keyword for ==>  JWT in Header 
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            //    //To Change from Route of Cookie ==> Account/Login
            //    //To JWT => UnAuth.
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            //    //For Any other behaviours ==> Auth. Filter search JWT 
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    //How to check if Token Valid 
            //    options.SaveToken = true; //Not Expired

            //    options.RequireHttpsMetadata = false; //specific protocole 

            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = builder.Configuration["JWT:IssuerIP"],
            //        ValidateAudience = true,
            //        ValidAudience = builder.Configuration["JWT:AudienceIP"],

            //        //Check Signature 
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecertKey"]))
            //    };


            //});





            //;






            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

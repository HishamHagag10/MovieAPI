
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Authentication;
using MovieAPI.Helper;
using MovieAPI.Meddlewares;
using MovieAPI.models;
using MovieAPI.Repository;
using System.Text;

namespace MovieAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDBContext>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var jwtOption = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
            builder.Services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOption.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOption.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SigningKey)),
                        
                    };
                });
            builder.Services.AddSingleton(jwtOption);
            
            //builder.Configuration.AddJsonFile("Path");
            builder.Services.Configure<AttachmentOption>(builder.Configuration.GetSection("Attachments"));


            //builder.Services.AddTransient<IRepository<Genre>, BaseRepository<Genre>>();
            //builder.Services.AddTransient<IRepository<Movie>, BaseRepository<Movie>>();
            builder.Services.AddTransient<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<PagenatedMapper>();
            builder.Services.AddAutoMapper(typeof(Program));
            
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMeddleware>();

            app.UseMiddleware<RateLimitingMiddleware>();
            
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}


namespace MovieAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddDbContext<AppDBContext>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);

            });


            JwtOptions jwtOption = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
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
            
            builder.Services.Configure<AttachmentOption>(builder.
                Configuration.GetSection("Attachments"));

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<AppDBContext>();
            builder.Services.AddScoped<PagenatedMapper>();
            builder.Services.AddScoped<ActorRepo>();
            builder.Services.AddScoped<AwardRepo>();
            builder.Services.AddScoped<GenreRepo>();
            builder.Services.AddScoped<MovieRepo>();
            builder.Services.AddScoped<ReviewRepo>();
            builder.Services.AddScoped<MovieActorsRepo>();
            builder.Services.AddScoped<ActorAwardRepo>();
            builder.Services.AddScoped<UserRepo>();
            builder.Services.AddScoped<UserMovieRepo>();
            builder.Services.AddScoped<RecommendationRepo>();


            builder.Services.AddAutoMapper(typeof(Program));
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

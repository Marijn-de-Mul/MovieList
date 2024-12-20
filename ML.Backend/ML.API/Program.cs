using Microsoft.EntityFrameworkCore;
using ML.DAL.Data;
using ML.DAL.Interfaces;
using ML.DAL.Repositories;
using ML.SAL.Interfaces;
using ML.SAL.Models;
using ML.SAL.Services;
using DotNetEnv;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;

if (!environment.IsProduction() && !environment.IsStaging())
{
    Env.Load(); 
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    if (!environment.IsProduction())
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); 
        });
    }
    
    if (environment.IsProduction())
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            policy.WithOrigins("http://movielist_frontend:3000", "http://movielist_frontend:3500", "https://movielist.marijndemul.nl")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); 
        });
    }
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMovieListService, MovieListService>();
builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieListRepository, MovieListRepository>();

builder.Services.AddScoped<IUser, User>(); 
builder.Services.AddScoped<IMovie, Movie>();
builder.Services.AddScoped<IMovieList, MovieList>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieList API", Version = "v1" });

    c.CustomSchemaIds(type => type.FullName);
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8079); 
    // options.ListenAnyIP(8080, listenOptions =>
    // {
    //     listenOptions.UseHttps("/etc/ssl/certs/mycertificate.crt", "/etc/ssl/private/mycertificate.key");
    // });
});

var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();
app.MapControllers();

app.Run();
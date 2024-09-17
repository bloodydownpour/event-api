using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using WebApplication1.Structure.Database;
using WebApplication1.Structure.JWT;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net.NetworkInformation;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using WebApplication1.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Token",
        Name = "Authorization",
        Type= SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddTransient<LoginUser>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddDbContext<EventDbContext>(x => x.UseSqlServer(
    "Data Source=(local); Database=ArticlesSite; Persist Security Info = false; MultipleActiveResultSets=True; Trusted_Connection=True; TrustServerCertificate=True;"
    ));
builder.Services.AddScoped<EventRepos>();
builder.Services.AddScoped<UserRepos>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>

    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCustomExceptionHandler();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "EventPhotos")),
    RequestPath="/EventPhotos"
});

app.UseStaticFiles(new StaticFileOptions
{

    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "UserPhotos")),
    RequestPath = "/UserPhotos"
});



app.MapControllers();

app.Run();

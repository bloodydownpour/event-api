using FluentValidation.AspNetCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

using EventList.API.Middleware;
using EventList.Infrastructure;
using System.Reflection;
using EventList.Application.Validation;
using EventList.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
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
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>

    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);
DataValidation.AddDataValidation(builder.Services);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrigin");
app.UseCustomExceptionHandler();

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

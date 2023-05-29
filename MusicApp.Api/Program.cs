using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MusicApp.Api.Common.Errors;
using MusicApp.Api.Errors;
using MusicApp.Api.MiddleWare;
using MusicApp.Application;
using MusicApp.Infrastructure;
using MusicApp.LocalStorage;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.





builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                          });
    });

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<HttpResponseExceptionFilter>();
    });

   
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    
    builder.Services.AddSwaggerGen(c =>
    {

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT" // Optional
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                new string[] {}
            }
        });
    });

    builder.Services.AddApplication()
                    .AddInfrastructure(builder.Configuration)
                    .AddLocalStorage(builder.Configuration);
    //builder.Services.AddSingleton<ProblemDetailsFactory, MusicAppProblemDetailsFactory>();
}
var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    //app.UseExceptionHandler("/error");
    app.UseCors(MyAllowSpecificOrigins);
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

  
    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();
    
    app.MapControllers();

    app.Run();
}

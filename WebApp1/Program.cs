using AspNetCore.Swagger.Themes;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace WebApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the database context
            builder.Services.AddDbContext<NotesDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SQLTestApp",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-8.0
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseSwaggerUI(ModernStyle.Dark);
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

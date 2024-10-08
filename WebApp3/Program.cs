using AspNetCore.Swagger.Themes;
using Microsoft.OpenApi.Models;
using WebApp3.Models;

namespace WebApp3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind the AzureBlobStorageSettings from appsettings.json
            builder.Services.Configure<AzureBlobModel>(builder.Configuration.GetSection("AzureBlobStorage"));

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BlobStorageTestApp",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
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

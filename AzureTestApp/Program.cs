using AspNetCore.Swagger.Themes;
using AzureTestApp.Models;
using AzureTestApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AzureTestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the database context
            builder.Services.AddDbContext<NotesDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add cosmos services to the container.
            builder.Services.Configure<CosmosDbSettingsModel>(builder.Configuration.GetSection("CosmosDb"));
            builder.Services.AddSingleton<CosmosDbService>();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AzureTestApp",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(ModernStyle.Dark);
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

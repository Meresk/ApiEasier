using ApiEasier.Server.DB;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Services;
using MongoDB.Driver;

namespace ApiEasier.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<JsonService>(provider => new JsonService("configuration"));
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<DBSettings>(
                builder.Configuration.GetSection("DatabaseSettings")
            );

            builder.Services.AddSingleton<MongoDBContext>();
            builder.Services.AddTransient<LogService>();
            builder.Services.AddScoped<IDynamicCollectionService, DynamicCollectionService>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}

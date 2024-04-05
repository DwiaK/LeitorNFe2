
using LeitorNFe.API.Configurations;
using LeitorNFe.Application;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Dispatcher;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.Persistence;
using Microsoft.OpenApi.Models;

namespace LeitorNFe.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Configuration.AddConfiguration(configuration);

            builder.Services.AddCors(policy =>
            {
                policy.AddPolicy("AllowSpecificOrigin", builder =>
                 builder.WithOrigins("http://localhost:7227/")
                  .SetIsOriginAllowed((host) => true) // Para endereço localhost
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
            });

            builder.Services
                .AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title = "LeitorNFe.API", Version = "v1" }));

            builder.Services.AddScoped<IDbConnection, DbConnectionFactory>();
			builder.Services.AddScoped<IDispatcher, Dispatcher>();

			builder.Services.AddHandlersFromAssembly(typeof(AssemblyReference).Assembly);

			var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

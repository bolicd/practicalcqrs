using System;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projections;
using Projections.ProjectionsImplementations;
using Serilog;

namespace ProjectionsHost
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<ISqlConnectionFactory>(x=>new SqlConnectionFactory("Server=(localdb)\\mssqllocaldb;Database=EventStoreDatabase;Trusted_Connection=True;"));
                services.AddScoped<IEventStore,EventStoreRepository>();
                services.AddScoped<IProjectionRepository, PersonProjectionRepository>();
                services.AddScoped<IProjection, PersonProjection>();
                services.AddHostedService<Worker>();
            }).UseSerilog();

    }
}

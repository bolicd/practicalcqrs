using System;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projections;
using Projections.ProjectionsImplementations;
using Serilog;
using PersonReadModelRepository = Projections.PersonReadModelRepository;

namespace ProjectionsHost
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("Projection Host starting ...");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<ISqlConnectionFactory>(x=>new SqlConnectionFactory("Server=(localdb)\\mssqllocaldb;Database=EventStoreDatabase;Trusted_Connection=True;"));
                services.AddScoped<IEventStore,EventStoreRepository>();
                services.AddScoped<IProjectionRepository, PersonReadModelRepository>();
                services.AddScoped<IProjection, PersonProjection>();
                services.AddScoped<IPersonReadModelRepository,PersonReadModelRepository>();
                services.AddHostedService<Worker>();
            }).UseSerilog();

    }
}

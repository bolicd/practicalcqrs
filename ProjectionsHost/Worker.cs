using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Projections;

namespace ProjectionsHost
{
    public class Worker: BackgroundService
    {
        private readonly IEnumerable<IProjection> _projections;
        private const int Take = 100;
        private const int PullingInterval = 1;

        public Worker(IEnumerable<IProjection> projections) => _projections = projections;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine("Worker started");

            try
            {
                //Log.Information("Starting projection agent");

                foreach (var projection in _projections)
                {
                    await projection.InitializeSequence().ConfigureAwait(false);

                    var sequenceInfo = $"Projection {projection} Sequence {((Projection)projection).Sequence}";
                    Console.WriteLine(sequenceInfo);
                    //Log.Information(sequenceInfo);
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    foreach (var projection in _projections)
                    {
                        await projection.ApplyEvents(Take).ConfigureAwait(false);
                    }

                    await Task.Delay(PullingInterval * 100, stoppingToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ex {e}");
                //Log.Fatal(e, $"Something went wrong in the projections. Error: {e.Message} Exception data: {e.Data} InnerException: {e.InnerException} Stacktrace {e.StackTrace}");
                throw;
            }
            finally
            {
                //Log.Information("Shutting down projection agent");
                //Log.CloseAndFlush();
            }
        }
    }
}

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
                foreach (var projection in _projections)
                {
                    await projection.InitializeSequence().ConfigureAwait(false);

                    // This would be a good place to log
                    var sequenceInfo = $"Projection {projection} Sequence {((Projection)projection).Sequence}";
                    Console.WriteLine(sequenceInfo);
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
                // this would be a good place to log exception, send notification to slack, email since problem with
                // projection usually stops projection agent and should be restarted/fixed asap
                Console.WriteLine($"ex {e}");
                throw;
            }
        }
    }
}

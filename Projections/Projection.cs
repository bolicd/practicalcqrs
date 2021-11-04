using System.Threading.Tasks;
using Core.Person.DomainEvents;

namespace Projections
{
    public abstract class Projection : IProjection
    {
        public int Sequence { get; private set; }

        private readonly IProjectionRepository _projectionRepository;

        protected async Task On(DomainEvent @event) => await UpdateSequence(@event.Sequence);

        protected Projection(IProjectionRepository projectionRepository)
        {
            _projectionRepository = projectionRepository;
        }

        public async Task InitializeSequence()
        {
            Sequence = await _projectionRepository.GetSequence().ConfigureAwait(false);
        }

        protected  Task UpdateSequence(int sequence)
        {
            Sequence = sequence;
            return Task.CompletedTask;
        }

        public async Task ApplyEvents(int take)
        {
            var events = await _projectionRepository.GetFromSequenceAsync(Sequence, take).ConfigureAwait(false);

            foreach (var @event in events)
            {
                await Apply(@event).ConfigureAwait(false);
            }
        }

        private async Task Apply(DomainEvent @event) => await ((dynamic)this).On((dynamic)@event);
    }
}

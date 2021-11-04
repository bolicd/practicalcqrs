using System.Threading.Tasks;
using Core.Person.DomainEvents;
using Infrastructure.Model.ReadModels;
using Infrastructure.Repositories;

namespace Projections.ProjectionsImplementations
{
    public class PersonProjection : Projection
    {
        private readonly IPersonReadModelRepository _personReadModelRepository;
        public PersonProjection(IPersonReadModelRepository personReadModelRepository) : base((IProjectionRepository)personReadModelRepository)
        {
            _personReadModelRepository = personReadModelRepository;
        }

        public async Task On(PersonCreated @event)
        {
            //write aggregate to normalized read model
            await _personReadModelRepository.SavePerson(new PersonReadModel()
            {
                EventId = @event.Id.ToString(),
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                Sequence = @event.Sequence,
                UpdatedAt = @event.CreatedAt,
                AggregateId = @event.AggregateId
            });

            await UpdateSequence(@event.Sequence);
        }

        public async Task On(AddressChanged @event)
        {
            // fetch aggregate
            var person = await _personReadModelRepository.GetPerson(@event.AggregateId);
            person.City = @event.City;
            person.Country = @event.Country;
            person.Street = @event.Street;
            person.ZipCode = @event.ZipCode;
            person.Sequence = @event.Sequence;
            person.UpdatedAt = @event.CreatedAt;
            
            await _personReadModelRepository.UpdatePerson(person);
            
            await UpdateSequence(@event.Sequence);
        }
    }
}

using System;
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
            try
            {
                await _personReadModelRepository.SavePerson(new PersonReadModel()
                {
                    EventId = @event.Id.ToString(),
                    FirstName = @event.FirstName,
                    LastName = @event.LastName,
                    Sequence = @event.Sequence,
                    UpdatedAt = @event.CreatedAt,
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }
    }
}

using System.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Model.ReadModels;
using Infrastructure.Repositories;

namespace Projections
{
    public class PersonReadModelRepository : ProjectionRepository,IPersonReadModelRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public PersonReadModelRepository(ISqlConnectionFactory connectionFactory,
            IEventStore eventStoreRepository) : base(connectionFactory, eventStoreRepository, "PersonReadModel")
        {
            _connectionFactory = connectionFactory;
        }

        public async Task SavePerson(PersonReadModel person)
        {
            await InsertAsync(new PersonReadModel
            {
                Sequence = person.Sequence,
                FirstName = person.FirstName,
                LastName = person.LastName,
                EventId = person.EventId,
                UpdatedAt = person.UpdatedAt
            });
        }

        public Task<PersonReadModel> GetPerson(string personId)
        {
            throw new System.NotImplementedException();
        }
    }
}

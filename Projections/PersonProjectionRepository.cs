using Infrastructure.Factories;
using Infrastructure.Repositories;

namespace Projections
{
    public class PersonProjectionRepository : ProjectionRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public PersonProjectionRepository(ISqlConnectionFactory connectionFactory,
            IEventStore eventStoreRepository) : base(connectionFactory, eventStoreRepository, "PersonReadModel")
        {
            _connectionFactory = connectionFactory;
        }
    }
}

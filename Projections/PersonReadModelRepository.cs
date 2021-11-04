using System.Threading.Tasks;
using Dapper;
using Infrastructure.Factories;
using Infrastructure.Model.ReadModels;
using Infrastructure.Repositories;

namespace Projections
{
    public class PersonReadModelRepository : ProjectionRepository,IPersonReadModelRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private const string Table= "PersonReadModel";
        private const string TableColumns = @"[Id],[FirstName],[LastName]," +
                                            "[Street],[City],[Country]," +
                                            "[ZipCode],[UpdatedAt]," +
                                            "[Sequence],[EventId],[AggregateId]";

        public PersonReadModelRepository(ISqlConnectionFactory connectionFactory,
            IEventStore eventStoreRepository) : base(connectionFactory, eventStoreRepository, Table)
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
                UpdatedAt = person.UpdatedAt,
                AggregateId = person.AggregateId
            });
        }

        public async Task UpdatePerson(PersonReadModel person)
        {
            await UpdateAsync(person);
        }


        public async Task<PersonReadModel> GetPerson(string personId)
        {
            await using var connection = _connectionFactory.SqlConnection();

            var query =
                $"SELECT TOP (1) {TableColumns} FROM [{Table}] WHERE [AggregateId]=@PersonId";

            return await connection.QueryFirstOrDefaultAsync<PersonReadModel>(query, new { PersonId = personId })
                .ConfigureAwait(false);
        }
    }
}

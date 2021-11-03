using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.Person.DomainEvents;
using Dapper;
using Infrastructure.Factories;
using Infrastructure.Repositories;

namespace Projections
{
    public abstract class ProjectionRepository: GenericRepository, IProjectionRepository
    {
        private readonly string _table;
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IEventStore _eventStoreRepository;

        public ProjectionRepository(ISqlConnectionFactory connectionFactory,
            IEventStore eventStoreRepository,
            string table) : base(connectionFactory,table)
        {
            _connectionFactory = connectionFactory;
            _eventStoreRepository = eventStoreRepository;
            _table = table;
        }

        public Task<SqlConnection> GetSqlConnection()
        {
            return Task.FromResult(_connectionFactory.SqlConnection());
        }
        public async Task<int> GetSequence()
        {
            var query = $"SELECT MAX (Sequence) FROM {_table}";

            await using var connection = _connectionFactory.SqlConnection();

            var sequence = await connection.QuerySingleOrDefaultAsync<int?>(query);

            return sequence ?? default;
        }
       

        public async Task<IReadOnlyCollection<DomainEvent>> GetFromSequenceAsync(int sequence, int take)
        {
            //var eventStoreRecords = await _eventStoreRepository
            //    .GetFromSequenceAsync<DomainEvent>(sequence, take).ConfigureAwait(false);
            var eventStoreRecords = await _eventStoreRepository.LoadAsyncFromOffset(sequence, take);

            //eventStoreRecords.ToList().ForEach(x => x.Event.WithVersionAndSequence(x.Version, x.Sequence));
            return eventStoreRecords.Select(x => x as DomainEvent).ToList().AsReadOnly();
        }
    }
}

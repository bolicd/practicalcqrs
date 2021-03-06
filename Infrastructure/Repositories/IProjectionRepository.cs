using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Person.DomainEvents;

namespace Infrastructure.Repositories
{
    public interface IProjectionRepository
    {
        Task<int> GetSequence();
        Task<IReadOnlyCollection<DomainEvent>> GetFromSequenceAsync(int sequence, int take);
    }
}

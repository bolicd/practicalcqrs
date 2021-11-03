using System.Threading.Tasks;
using Infrastructure.Model.ReadModels;

namespace Infrastructure.Repositories
{
    public interface IPersonReadModelRepository
    {
        Task SavePerson(PersonReadModel person);

        Task<PersonReadModel> GetPerson(string personId);
    }
}

using System.Threading.Tasks;

namespace Projections
{
    public interface IProjection
    {
        Task InitializeSequence();
        Task ApplyEvents(int take);
    }
}

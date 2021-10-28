using System;
using System.Threading.Tasks;
using Core.Person.DomainEvents;

namespace Projections.ProjectionsImplementations
{
    public class PersonProjection : Projection
    {
        public PersonProjection(IProjectionRepository projectionRepository) : base(projectionRepository)
        {
        }

        public async Task On(PersonCreated @event)
        {
            Console.WriteLine("Person Created");
        }
    }
}

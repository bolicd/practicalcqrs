using System;
using Newtonsoft.Json;

namespace Core.Person.DomainEvents
{
    public class PersonCreated : DomainEvent
    {
       
        [JsonProperty("firstName")]
        public string FirstName { get; }
        [JsonProperty("lastName")]
        public string LastName { get; }

        public PersonCreated(
            string aggregateId,
            string firstName, 
            string lastName) : base(aggregateId)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        [JsonConstructor]
        public PersonCreated(string aggregateId,
            Guid id,
            DateTime createdAt,
            string firstName,
            string lastName) : base(id, aggregateId, createdAt)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}

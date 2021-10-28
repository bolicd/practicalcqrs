using System;
using Newtonsoft.Json;
using Tactical.DDD;

namespace Core.Person.DomainEvents
{
    public abstract class DomainEvent : IDomainEvent
    {
        [JsonProperty("id")]
        public Guid Id { get; }

        [JsonProperty("aggregate_id")]
        public string AggregateId { get; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int Version { get; private set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int Sequence { get; private set; }

        protected DomainEvent(string aggregateId)
        {
            (Id, AggregateId, CreatedAt, Version, Sequence) = (Guid.NewGuid(), aggregateId, DateTime.Now, 0, 0);
        }

        [JsonConstructor]
        protected DomainEvent(Guid id, string aggregateId, DateTime createdAt)
        {
            (Id, AggregateId, CreatedAt) = (id, aggregateId, createdAt);
        }

        public DateTime CreatedAt { get; set; }

        public void WithVersionAndSequence(int version, int sequence)
        {
            (Version, Sequence) = (version, sequence);
        }
    }
}

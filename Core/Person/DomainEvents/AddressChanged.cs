using System;
using Newtonsoft.Json;

namespace Core.Person.DomainEvents
{
    public class AddressChanged : DomainEvent
    {
        [JsonProperty("city")]
        public string City { get; }
        [JsonProperty("country")]
        public string Country { get; }
        [JsonProperty("zipCode")]
        public string ZipCode { get; }

        [JsonProperty("street")]
        public string Street { get; }

        public AddressChanged(string aggregateId, string city,
            string country,
            string zipcode,
            string street) : base(aggregateId)
        {
            City = city;
            Country = country;
            ZipCode = zipcode;
            Street = street;
        }

        [JsonConstructor]
        public AddressChanged(string aggregateId,
            Guid id,
            DateTime createdAt,
            string city,
            string country,
            string zipcode,
            string street) : base(id, aggregateId, createdAt)
        {
            City = city;
            Country = country;
            ZipCode = zipcode;
            Street = street;
        }
    }
}


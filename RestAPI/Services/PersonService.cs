using System;
using Core.Person;
using Core.Person.Repositories;
using RestAPI.Model;
using System.Threading.Tasks;
using Infrastructure.Repositories;

namespace RestAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonReadModelRepository _personReadModelRepository;
        public PersonService(IPersonRepository personRepository, IPersonReadModelRepository personReadModelRepository)
        {
            _personRepository = personRepository;
            _personReadModelRepository = personReadModelRepository;
        }

        public async Task<PersonId> CreatePerson(string firstName, string lastName)
        {
            var person = Person.CreateNewPerson(new PersonId(),firstName, lastName);
            var pid=  await _personRepository.SavePersonAsync(person);
            return pid;
        }

        public async Task<PersonDto> GetPerson(string personId)
        {
            //var person = await _personRepository.GetPerson(personId);
            
            //Read person from read model
            var person = await _personReadModelRepository.GetPerson(personId);

            if (person == null) return new PersonDto(); // throw not found exception

            return new PersonDto
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                PersonId = person.Id.ToString(),
                Address = new AddressDto
                {
                    City = person.City,
                    Country = person.Country,
                    Street = person.Street,
                    ZipCode = person.ZipCode
                }
            };
        }


        public async Task UpdatePersonAddress(PersonId personId, string city, string country, string street, string zipcode)
        {
            var person = await _personRepository.GetPerson(personId.ToString());
         
            if (person == null) return; // throw person not found exception

            person.ChangePersonAddress(street, country, zipcode, city);
            await _personRepository.SavePersonAsync(person);
        }
    }
}

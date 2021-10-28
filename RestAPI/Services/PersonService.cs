﻿using System;
using Core.Person;
using Core.Person.Repositories;
using RestAPI.Model;
using System.Threading.Tasks;

namespace RestAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonId> CreatePerson(string firstName, string lastName)
        {
            var person = Person.CreateNewPerson(new PersonId(),firstName, lastName);
            var pid=  await _personRepository.SavePersonAsync(person);
            return pid;
        }

        public async Task<PersonDto> GetPerson(string personId)
        {
            var person = await _personRepository.GetPerson(personId);
            
            if (person == null) return new PersonDto(); // throw not found exception

            return new PersonDto()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                PersonId = person.Id.ToString(),
                Address = person.PersonAddress!=null ? new AddressDto()
                {
                    City = person.PersonAddress?.City,
                    Country = person.PersonAddress?.Country,
                    Street = person.PersonAddress?.Street,
                    ZipCode = person.PersonAddress?.ZipCode
                } : null
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

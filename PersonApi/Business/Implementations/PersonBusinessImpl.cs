using System;
using System.Collections.Generic;
using System.Linq;
using PersonApi.Business;
using PersonApi.Models;
using PersonApi.Models.Context;
using PersonApi.Repository;

namespace PersonApi.Business.Implementations
{
    public class PersonBusinessImpl : IPersonBusiness
    {

        private readonly IPersonRepository _repository;

        public PersonBusinessImpl(IPersonRepository repository)
        {
            _repository = repository;
        }

        public Person Create(Person person)
        {
            return _repository.Create(person);
        }

        public List<Person> FindAll()
        {
            return _repository.FindAll();
        }

        public Person FindById(long id)
        {
            return _repository.FindById(id);
        }

        public Person Update(Person person)
        {
            return _repository.Update(person);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

    }
}
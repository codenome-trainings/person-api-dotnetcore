using System.Collections.Generic;
using PersonApi.Models;
using PersonApi.Models.Base;

namespace PersonApi.Repository.Generics
{
    public interface IPersonRepository : IRepository<Person>
    {
        List<Person> FindByName(string firstName, string lastName);
    }
}
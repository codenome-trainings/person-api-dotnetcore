using System.Collections.Generic;
using System.Linq;
using PersonApi.Models;
using PersonApi.Models.Context;
using PersonApi.Repository.Generics;

namespace PersonApi.Repository.Implementations
{
    public class PersonRepositoryImpl : GenericRepository<Person>, IPersonRepository
    {

        public PersonRepositoryImpl(MySQLContext context) : base (context)
        {
        }

        public List<Person> FindByName(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return _context.Persons.Where(p => p.FirstName.Contains(firstName) && p.LastName.Contains(firstName)).ToList();            
            }
            else if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return _context.Persons.Where(p => p.LastName.Contains(firstName)).ToList();
            }
            else if (!string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return _context.Persons.Where((p) => p.FirstName.Contains(firstName)).ToList();
            }
            else
            {
                return _context.Persons.ToList();
            }
        }
    }
}
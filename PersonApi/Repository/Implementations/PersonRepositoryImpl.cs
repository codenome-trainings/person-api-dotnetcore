using System;
using System.Collections.Generic;
using System.Linq;
using PersonApi.Models;
using PersonApi.Models.Context;

namespace PersonApi.Repository.Implementations
{
    public class PersonRepositoryImpl : IPersonRepository
    {
        private readonly MySQLContext _context;

        public PersonRepositoryImpl(MySQLContext context)
        {
            _context = context;
        }

        public Person Create(Person person)
        {
            try
            {
                _context.Add(person);
                _context.SaveChanges();
                return person;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Person FindById(long id)
        {
            try
            {
                var person = _context.Persons.SingleOrDefault(
                    p => p.Id.Equals(id)
                );
                return person;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public List<Person> FindAll()
        {
            try
            {
                var persons = _context.Persons.ToList();
                return persons;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public Person Update(Person person)
        {
            if (!Exists(person.Id)) return null;

            try
            {
                var result = _context.Persons.SingleOrDefault(
                    p => p.Id.Equals(person.Id)
                );
                _context.Entry(result).CurrentValues.SetValues(person);
                _context.SaveChanges();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public void Delete(long id)
        {
            try
            {
                var result = _context.Persons.SingleOrDefault(
                    p => p.Id.Equals(id)
                );

                if (result != null) _context.Persons.Remove(result);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool Exists(long? id)
        {
            return _context.Persons.Any(
                p => p.Id.Equals(id)
            );
        }
    }
}
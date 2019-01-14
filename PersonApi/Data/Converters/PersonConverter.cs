using System.Collections.Generic;
using System.Linq;
using PersonApi.Data.VO;
using PersonApi.Models;

namespace PersonApi.Data.Converters
{
    public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
    {
        public PersonVO Parse(Person origim)
        {
            if (origim == null) return new PersonVO();
            return new PersonVO
            {
                Id = origim.Id,
                FirstName = origim.FirstName,
                LastName = origim.LastName,
                Address = origim.Address,
                Gender = origim.Gender
            };
        }

        public List<PersonVO> ParseList(List<Person> origin)
        {
            if (origin == null) return new List<PersonVO>();
            return origin.Select(item => Parse(item)).ToList();
        }

        public Person Parse(PersonVO origim)
        {
            if (origim == null) return new Person();
            return new Person
            {
                Id = origim.Id,
                FirstName = origim.FirstName,
                LastName = origim.LastName,
                Address = origim.Address,
                Gender = origim.Gender
            };
        }

        public List<Person> ParseList(List<PersonVO> origin)
        {
            if (origin == null) return new List<Person>();
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}
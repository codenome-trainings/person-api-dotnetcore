using System.Collections.Generic;
using PersonApi.Data.Converters;
using PersonApi.Data.VO;
using PersonApi.Models;
using PersonApi.Repository.Generics;
using Tapioca.HATEOAS.Utils;

namespace PersonApi.Business.Implementations
{
    public class PersonBusinessImpl : IPersonBusiness
    {
        private readonly PersonConverter _converter;
        private readonly IPersonRepository _repository;

        public PersonBusinessImpl(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public PersonVO Create(PersonVO person)
        {
            var personEntity = _converter.Parse(person);
            personEntity = _repository.Create(personEntity);
            return _converter.Parse(personEntity);
        }

        public List<PersonVO> FindAll()
        {
            return _converter.ParseList(_repository.FindAll());
        }

        public List<PersonVO> FindByName(string firstName, string lastName)
        {
            return _converter.ParseList(_repository.FindByName(firstName, lastName));
        }

        public PersonVO FindById(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }

        public PersonVO Update(PersonVO person)
        {
            var personEntity = _converter.Parse(person);
            personEntity = _repository.Update(personEntity);
            return _converter.Parse(personEntity);
        }

        public PagedSearchDTO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {

            page = page > 0 ? page - 1 : 0;
            string query = @"select * from persons p where 1 = 1 ";
            if (!string.IsNullOrEmpty(name)) query = query + $"and p.firstname like '%{name}%' ";
            
            query = query + $" order by p.firstname {sortDirection} limit {pageSize} offset {page}";
            
            string countQuery = @"select count(*) from persons p where 1 = 1 ";
            if (!string.IsNullOrEmpty(name)) countQuery = countQuery + $" and p.name like '%{name}%' ";
            
            var persons = _repository.FindWithPagedSearch(query);
            
            int totalResults = _repository.GetCount(countQuery);
            
            return new PagedSearchDTO<PersonVO>
            {
                CurrentPage = page + 1,
                List = _converter.ParseList(persons),
                PageSize = pageSize,
                SortDirections = sortDirection,
                TotalResults = totalResults
            };
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
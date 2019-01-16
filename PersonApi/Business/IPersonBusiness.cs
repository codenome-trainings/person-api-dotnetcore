using System.Collections.Generic;
using PersonApi.Data.VO;
using PersonApi.Models;
using Tapioca.HATEOAS.Utils;

namespace PersonApi.Business
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);

        List<PersonVO> FindAll();
        
        List<PersonVO> FindByName(string firstName, string lastName);

        PersonVO FindById(long id);

        PersonVO Update(PersonVO person);

        void Delete(long id);

        PagedSearchDTO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
    }
}
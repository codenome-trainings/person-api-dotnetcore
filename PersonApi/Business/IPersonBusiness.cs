using System.Collections.Generic;
using PersonApi.Models;

namespace PersonApi.Business
{
    public interface IPersonBusiness
    {
        Person Create(Person person);

        List<Person> FindAll();

        Person FindById(long id);

        Person Update(Person person);
        
        void Delete(long id);
    }
}
using System.Collections.Generic;
using PersonApi.Models;

namespace PersonApi.Repository
{
    public interface IUserRepository
    {
        User FindByLogin(string login);
    }
}
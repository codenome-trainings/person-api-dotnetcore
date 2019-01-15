using System;
using System.Collections.Generic;
using System.Linq;
using PersonApi.Models;
using PersonApi.Models.Context;

namespace PersonApi.Repository.Implementations
{
    public class UserRepositoryImpl : IUserRepository
    {

        private readonly MySQLContext _context;

        public UserRepositoryImpl(MySQLContext context)
        {
            _context = context;
        }


        public User FindByLogin(string login)
        {
            return _context.Users.SingleOrDefault(u => u.Login.Equals(login));
        }
    }
}
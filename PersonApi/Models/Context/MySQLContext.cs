using Microsoft.EntityFrameworkCore;

namespace PersonApi.Models.Context
{
    public class MySQLContext : DbContext
    {
        protected MySQLContext()
        {
        }

        public MySQLContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
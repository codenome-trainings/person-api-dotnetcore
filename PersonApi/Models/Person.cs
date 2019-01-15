using System.ComponentModel.DataAnnotations.Schema;
using PersonApi.Models.Base;

namespace PersonApi.Models
{
    [Table("persons")]
    public class Person : BaseEntity
    {
        [Column("firstname")] public string FirstName { get; set; }

        [Column("lastname")] public string LastName { get; set; }

        [Column("address")] public string Address { get; set; }

        [Column("gender")] public string Gender { get; set; }
        
    }
}
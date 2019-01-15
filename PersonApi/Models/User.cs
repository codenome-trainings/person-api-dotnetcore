using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MySqlX.XDevAPI.Relational;

namespace PersonApi.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }
        
        [Column("login")]
        public string Login { get; set; }
        
        [Column("access_key")]
        public string AccessKey { get; set; }
    }
}
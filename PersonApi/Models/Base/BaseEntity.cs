using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonApi.Models.Base
{
    //[DataContract]
    public class BaseEntity
    {
        [Key] [Column("id")] public long? Id { get; set; }
    }
}
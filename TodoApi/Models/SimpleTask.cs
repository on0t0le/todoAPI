using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    [Table("todos")]
    public class SimpleTask
    {
        
        [Column("id")]
        [Key]
        public int id { get; set; }
        [Column("task")]
        public string task { get; set; }
        [Column("isdone")]
        public bool isDone { get; set; }
    }
}

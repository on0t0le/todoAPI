using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    [Table("todos")]
    public class SimpleTask
    {
        [Column("id")]
        public int id { get; set; }
        [Column("task")]
        public string task { get; set; }
    }
}

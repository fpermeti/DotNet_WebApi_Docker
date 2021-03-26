using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Author")]
        public string Author { get; set; }

        [Required]
        [Range(0, 1000000)]
        [Column("Price")]
        public double Price { get; set; }
    }
}

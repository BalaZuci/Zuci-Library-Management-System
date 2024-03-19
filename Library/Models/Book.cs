using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    /// <summary>
    /// Book class contain the properties of Books.
    /// createdBy, CreatedOn, LastUpdatedOn are some other properties for reviewing.
    /// </summary>
    [Table("Book")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string? AuthorName { get; set; }
        public DateTime PublicationYear { get; set; }

        [MaxLength(45)]
        public string? Genre { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 1000)]
        public int QuantityAvailable { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CreatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}

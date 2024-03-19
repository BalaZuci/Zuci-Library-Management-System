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
    /// Borrower class contains the properties of Borrower.
    /// createdBy, CreatedOn, LastUpdatedOn are some other properties for reviewing.
    /// </summary>
    [Table("Borrower")]
    public class Borrower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BorrowerId { get; set; }

        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        [Required]
        [StringLength(11)]
        public string EmpId { get; set; }

        [MaxLength(25)]
        public string? Department { get; set; }

        [Required]
        [MaxLength(10)]
        public string BorrowerType { get; set; }

        [Required]
        [MaxLength(45)]
        public string Email { get; set; }

        [Required]
        [MaxLength(45)]
        public string Password { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime LastUpdatedOn { get; set; }
    }
}

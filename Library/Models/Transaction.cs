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
    /// Transaction Class contains Properties like BookId, BorrowerId, which are foreign Keys.
    /// Borrow or Return Of any Book By any Borrower is listed in transaction Table.
    /// </summary>
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        [ForeignKey("Borrower")]
        public int BorrowerId { get; set; }

        [Required]
        public string TransactionType { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        [ForeignKey("BorrowerId")]
        public virtual Borrower? Borrower { get; set; }
    }
}

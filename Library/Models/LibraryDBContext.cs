using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class LibraryDBContext : DbContext
    {
        public LibraryDBContext()
        {

        }
        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options)
        {

        }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Borrower> Borrowers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=ZSCHN01LP0308;Initial Catalog=LibraryDB;User ID=sa;Password=Password@123;Trust Server Certificate=True");
        }
    }
}
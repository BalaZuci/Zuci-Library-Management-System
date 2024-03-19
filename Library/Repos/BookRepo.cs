using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repos
{
    /// <summary>
    /// This BookRepo Class will implement all the Book DataBase related actions.
    /// this class also contains all the admin and user Methods
    /// </summary>
    public class BookRepo : IBookRepo
    {
        LibraryDBContext cnt = new LibraryDBContext();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="borrowerId"></param>
        /// <returns></returns>
        /// <exception cref="LibraryException"></exception>
        public async Task<List<Book>> BorrowerBooks(int borrowerId)
        {
            try
            {
                List<Book> books = new List<Book>();

                List<Transaction> transactions = await (from t in cnt.Transactions where t.BorrowerId == borrowerId select t).ToListAsync();
                foreach (Transaction transaction in transactions)
                {
                    int borrowCount = await cnt.Transactions.CountAsync(t => (t.BorrowerId == borrowerId) && (t.BookId == transaction.BookId) && (t.TransactionType == "Borrow"));
                    int returnCount = await cnt.Transactions.CountAsync(t => (t.BorrowerId == borrowerId) && (t.BookId == transaction.BookId) && (t.TransactionType == "Return"));
                    if (borrowCount > returnCount)
                    {
                        books.Add(await GetBookById(transaction.BookId));
                    }
                }
                return books.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }
        public async Task BorrowBook(int bookId, int borrowerId)
        {
            int borrowCount = await cnt.Transactions.CountAsync(t => (t.BorrowerId == borrowerId) && (t.BookId == bookId) && (t.TransactionType == "Borrow"));
            int returnCount = await cnt.Transactions.CountAsync(t => (t.BorrowerId == borrowerId) && (t.BookId == bookId) && (t.TransactionType == "Return"));
            if (borrowCount > returnCount)
            {
                throw new LibraryException("You have already taken a copy of this book. So, Please choose another book.");
            }

            Book book = await GetBookById(bookId);
            if (book.QuantityAvailable <= 0)
            {
                throw new LibraryException("No Books are present now.");
            }

            try
            {
                book.QuantityAvailable -= 1;
                Transaction transaction = new Transaction();
                transaction.BookId = bookId;
                transaction.BorrowerId = borrowerId;
                transaction.TransactionType = "Borrow";
                transaction.TransactionDate = DateTime.Now;
                transaction.DueDate = transaction.TransactionDate.AddDays(15);

                await cnt.Transactions.AddAsync(transaction);
                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }

        public async Task DeleteBook(int bookId)
        {
            try
            {
                Book book = await GetBookById(bookId);
                cnt.Remove(book);
                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }

        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> books = await cnt.Books.ToListAsync();
            return books;
        }

        public async Task<Book> GetBookById(int bookId)
        {
            try
            {
                Book book = await (from b in cnt.Books where b.BookId == bookId select b).FirstAsync();
                return book;
            }
            catch
            {
                throw new LibraryException("No Such Book Id exists.");
            }
        }

        public async Task<List<Book>> GetByAuthor(string author)
        {
            try
            {
                List<Book> books = await (from b in cnt.Books where b.AuthorName == author select b).ToListAsync();
                return books;
            }
            catch
            {
                throw new LibraryException("No Such Author Name exists.");
            }
        }

        public async Task<List<Book>> GetByGenre(string genre)
        {
            try
            {
                List<Book> books = await (from b in cnt.Books where b.Genre == genre select b).ToListAsync();
                return books;
            }
            catch
            {
                throw new LibraryException("No Such Genre exists.");
            }
        }

        public async Task<List<Book>> GetByPublicationYear(DateTime publicationYear)
        {
            try
            {
                List<Book> books = await (from b in cnt.Books where b.PublicationYear.Year == publicationYear.Year select b).ToListAsync();
                return books;
            }
            catch
            {
                throw new LibraryException("No Such Publication Year exists.");
            }
        }

        public async Task InsertBook(Book book)
        {
            try
            {
                book.CreatedOn = DateTime.Now;
                book.LastUpdatedOn = book.CreatedOn;
                await cnt.Books.AddAsync(book);
                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }

        public async Task ReturnBook(int bookId, int borrowerId)
        {
            int borrowCount = await cnt.Transactions.CountAsync(t => (t.BorrowerId == borrowerId) && (t.BookId == bookId) && (t.TransactionType == "Borrow"));
            int returnCount = await cnt.Transactions.CountAsync(t => (t.BorrowerId == borrowerId) && (t.BookId == bookId) && (t.TransactionType == "Return"));
            if (borrowCount <= returnCount)
            {
                throw new LibraryException("You have not taken this Book.");
            }

            try
            {
                Transaction borrowTransaction = await (from t in cnt.Transactions where t.BookId == bookId && t.BorrowerId == borrowerId orderby t.TransactionDate select t).LastAsync();
                Book book = await GetBookById(bookId);
                book.QuantityAvailable += 1;

                Transaction transaction = new Transaction();
                transaction.BookId = bookId;
                transaction.BorrowerId = borrowerId;
                transaction.TransactionType = "Return";
                transaction.TransactionDate = DateTime.Now;
                transaction.DueDate = borrowTransaction.DueDate;

                await cnt.Transactions.AddAsync(transaction);
                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }

        public async Task UpdateBook(int bookId, Book book)
        {
            try
            {
                Book bookToEdit = await GetBookById(bookId);
                bookToEdit.Title = book.Title;
                bookToEdit.Description = book.Description;
                bookToEdit.AuthorName = book.AuthorName;
                bookToEdit.PublicationYear = book.PublicationYear;
                bookToEdit.Genre = book.Genre;
                bookToEdit.QuantityAvailable = book.QuantityAvailable;
                bookToEdit.LastUpdatedOn = DateTime.Now;

                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }
    }
}

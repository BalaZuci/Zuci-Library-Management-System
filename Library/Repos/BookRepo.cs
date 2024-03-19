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
        /// For getting list of books the borrower had at present.
        /// </summary>
        /// <param name="borrowerId">Borrower Id the primary key, getting from the current session</param>
        /// <returns>List of Books that borrower had at the moment</returns>
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
        /// <summary>
        /// To borrow a book if it is present and not taken any copy of the particular book.
        /// </summary>
        /// <param name="bookId">bookId specifies the Book the borrower need to borrow</param>
        /// <param name="borrowerId">boorowerId specifies the Boorower identification</param>
        /// <returns>returns void</returns>
        /// <exception cref="LibraryException"> any exception which may occur</exception>
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
        /// <summary>
        /// to delete the book from database
        /// </summary>
        /// <param name="bookId">bookId tells which book to delete</param>
        /// <returns>nothing</returns>
        /// <exception cref="LibraryException">normal exception</exception>
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
        /// <summary>
        /// for getting all books present in the database
        /// </summary>
        /// <returns>List of Books</returns>
        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> books = await cnt.Books.ToListAsync();
            return books;
        }
        /// <summary>
        /// Gets details of particular book
        /// </summary>
        /// <param name="bookId">bookId specifies which book is needed</param>
        /// <returns>Book object</returns>
        /// <exception cref="LibraryException">if book doesn't exists return exception</exception>
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
        /// <summary>
        /// for getting the particular books written by particular author
        /// </summary>
        /// <param name="author">author name</param>
        /// <returns>List of books</returns>
        /// <exception cref="LibraryException">throws exception, if author name doesn't exists</exception>
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
        /// <summary>
        /// for getting the books of particular genre
        /// </summary>
        /// <param name="genre">genre name for searching in database</param>
        /// <returns>List of books</returns>
        /// <exception cref="LibraryException">throws exception if genre doesn't exists</exception>
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
        /// <summary>
        /// for getting the books publicated on the particular year
        /// </summary>
        /// <param name="publicationYear">it contains the year</param>
        /// <returns>list of books</returns>
        /// <exception cref="LibraryException">throws exception, something went wrong</exception>
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
        /// <summary>
        /// returns book, adds a transaction in transaction table
        /// </summary>
        /// <param name="bookId">which book to return </param>
        /// <param name="borrowerId">who is returning the book</param>
        /// <returns>void</returns>
        /// <exception cref="LibraryException">throws error if book is not taken, if anything went worng</exception>
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
        /// <summary>
        /// this method will update the book details
        /// </summary>
        /// <param name="bookId">specifies the book to be edited</param>
        /// <param name="book">book object contains updated details of the book</param>
        /// <returns>void</returns>
        /// <exception cref="LibraryException">throws exception if book not found or something went wrong</exception>
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
using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace LibraryWebApi.Controllers
{
    /// <summary>
    /// this Controller is used to control the Admin operations on Book. 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookAdminController : ControllerBase
    {
        /// <summary>
        /// Dependency injection
        /// </summary>
        IBookRepo repo;
        public BookAdminController(IBookRepo bookRepo)
        {
            repo = bookRepo;
        }

        /// <summary>
        /// this endpoint gets books from library
        /// </summary>
        /// <returns>list of books</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            List<Book> books = await repo.GetAllBooks();
            return Ok(books);
        }

        /// <summary>
        /// gets Borrowed Books of the particular borrower
        /// </summary>
        /// <param name="borrowerId">Specifies the borrower</param>
        /// <returns>List of books</returns>
        [HttpGet("BorrowedBooks/{borrowerId}")]
        public async Task<ActionResult> GetBorrowedBooks(int borrowerId)
        {
            try
            {
                List<Book> books = await repo.BorrowerBooks(borrowerId);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Gets book by its Id
        /// </summary>
        /// <param name="bookId">specifies the book</param>
        /// <returns>Book Object</returns>
        [HttpGet("ByBookId/{bookId}")]
        public async Task<ActionResult> GetBookById(int bookId)
        {
            try
            {
                Book book = await repo.GetBookById(bookId);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get books written by particular author
        /// </summary>
        /// <param name="author">specifies the author name</param>
        /// <returns>list of books</returns>
        [HttpGet("ByAuthor/{author}")]
        public async Task<ActionResult> GetByAuthor(string author)
        {
            try
            {
                List<Book> books = await repo.GetByAuthor(author);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get books publicated on particular year
        /// </summary>
        /// <param name="publicationYear">specifies the publication year</param>
        /// <returns>list of books</returns>
        [HttpGet("ByPublicationYear/{publicationYear}")]
        public async Task<ActionResult> GetByPublicationYear(DateTime publicationYear)
        {
            try
            {
                List<Book> books = await repo.GetByPublicationYear(publicationYear);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Gets books of particular genre
        /// </summary>
        /// <param name="genre">specifies the genre name</param>
        /// <returns>list of books</returns>
        [HttpGet("ByGenre/{genre}")]
        public async Task<ActionResult> GetByGenre(string genre)
        {
            try
            {
                List<Book> books = await repo.GetByGenre(genre);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// this endpoint is used to borrow book 
        /// </summary>
        /// <param name="bookId">specifies the book</param>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>nothing</returns>
        [HttpPost("ForBorrow/{bookId}/{borrowerId}")]
        public async Task<ActionResult> BorrowBook(int bookId, int borrowerId)
        {
            try
            {
                await repo.BorrowBook(bookId, borrowerId);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// this endpoint is used to return book
        /// </summary>
        /// <param name="bookId">specifies the book</param>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>nothing</returns>
        [HttpPost("ForReturn/{bookId}/{borrowerId}")]
        public async Task<ActionResult> ReturnBook(int bookId, int borrowerId)
        {
            try
            {
                await repo.ReturnBook(bookId, borrowerId);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// this endpoint inserts a new book
        /// </summary>
        /// <param name="book">Book object to be inserted</param>
        /// <returns>nothing</returns>
        [HttpPost]
        public async Task<ActionResult> InsertBook(Book book)
        {
            try
            {
                await repo.InsertBook(book);
                return Created($"api/BookAdmin/{book.BookId}", book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// this endpoint is used to update the details of the already existing book
        /// </summary>
        /// <param name="bookId">specifies the book to be edited</param>
        /// <param name="book">Contains the updated book details</param>
        /// <returns>nothing</returns>
        [HttpPut("{bookId}")]
        public async Task<ActionResult> UpdateBook(int bookId, Book book)
        {
            try
            {
                await repo.UpdateBook(bookId, book);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// this end point deletes the Book
        /// </summary>
        /// <param name="bookId">specifies the Book</param>
        /// <returns>nothing</returns>
        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            try
            {
                await repo.DeleteBook(bookId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

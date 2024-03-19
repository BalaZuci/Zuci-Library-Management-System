using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    /// <summary>
    /// this Controller is used to control Book by User
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookUserController : ControllerBase
    {
        /// <summary>
        /// Dependency injection
        /// </summary>
        IBookRepo repo;
        public BookUserController(IBookRepo bookUserRepo)
        {
            repo = bookUserRepo;
        }

        /// <summary>
        /// For getting all the books
        /// </summary>
        /// <returns>list of books</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            List<Book> books = await repo.GetAllBooks();
            return Ok(books);
        }
        /// <summary>
        /// list of books borrowed at present
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>list of books</returns>
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
        /// Getting book by its Id
        /// </summary>
        /// <param name="bookId">specifies the Book</param>
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
        /// Getting books written by particular author
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
        /// Getting books publicated on the particular year
        /// </summary>
        /// <param name="publicationYear">contains the particular year</param>
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
        /// Getting books of particular genre
        /// </summary>
        /// <param name="genre">specifies the name of the genre</param>
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
        /// For borrowing book
        /// </summary>
        /// <param name="bookId">specifies the book to be borrowed</param>
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
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// for returning the book
        /// </summary>
        /// <param name="bookId">Specifies the Book to be returned</param>
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
    }
}

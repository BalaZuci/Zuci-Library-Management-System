using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAdminController : ControllerBase
    {
        IBookRepo repo;
        public BookAdminController(IBookRepo bookRepo)
        {
            repo = bookRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            List<Book> books = await repo.GetAllBooks();
            return Ok(books);
        }

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

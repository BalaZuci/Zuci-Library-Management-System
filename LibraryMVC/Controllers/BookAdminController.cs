using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace LibraryMVC.Controllers
{
    /// <summary>
    /// Controller to control the view of Book Page by Admin
    /// </summary>
    public class BookAdminController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BookAdmin/") };
        /// <summary>
        /// For showing all Books
        /// </summary>
        /// <returns>view of all books</returns>
        public async Task<ActionResult> Index()
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>("");
            return View(books);
        }
        /// <summary>
        /// for showing the borrowed books
        /// </summary>
        /// <returns>view of boroowed books</returns>
        public async Task<ActionResult> GetByBorrowedBooks()
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"BorrowedBooks/{borrowerId}");
            return View(books);
        }
        /// <summary>
        /// for showing details of a particular book
        /// </summary>
        /// <param name="bookId">specifies the book</param>
        /// <returns>view of details of the book</returns>
        public async Task<ActionResult> Details(int bookId)
        {
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");
            return View(book);
        }
        /// <summary>
        /// for showing list of books written by particular author
        /// </summary>
        /// <param name="author">specifies the author name</param>
        /// <returns>view of Books</returns>
        public async Task<ActionResult> GetByAuthor(string author)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByAuthor/{author}");
            return View(books);
        }
        /// <summary>
        /// for showing List of books publicated on particular year
        /// </summary>
        /// <param name="publicationYear">contains the year</param>
        /// <returns>view of list of books</returns>
        public async Task<ActionResult> GetByPublicationYear(DateTime publicationYear)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByPublicationYear/{publicationYear}");
            return View(books);
        }
        /// <summary>
        /// for showing list of books belongs to particular genre
        /// </summary>
        /// <param name="genre">specifies the genre name</param>
        /// <returns>view of list of books</returns>
        public async Task<ActionResult> GetByGenre(string genre)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByGenre/{genre}");
            return View(books);
        }
        /// <summary>
        /// for showing borrowing status of a book by a particular user
        /// </summary>
        /// <param name="bookId">specifies a book</param>
        /// <returns>view of borrow status of the book</returns>
        public async Task<ActionResult> BorrowBook(int bookId)
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");

            var response = await svc.PostAsJsonAsync<Book>($"ForBorrow/{bookId}/{borrowerId}", book);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Status = true;
            }
            else
            {
                string meassage = Regex.Replace(response.Content.ReadAsStringAsync().Result, "[^a-zA-Z0-9\\s.,]", "");
                ViewBag.Message = meassage;
                ViewBag.Status = false;
            }
            return View(book);
        }
        /// <summary>
        /// for showing returning status of a book by a particular user
        /// </summary>
        /// <param name="bookId">specifies a book</param>
        /// <returns>view of return status of the book</returns>
        public async Task<ActionResult> ReturnBook(int bookId)
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");

            var response = await svc.PostAsJsonAsync<Book>($"ForReturn/{bookId}/{borrowerId}", book);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Status = true;
            }
            else
            {
                string meassage = Regex.Replace(response.Content.ReadAsStringAsync().Result, "[^a-zA-Z0-9\\s.,]", "");
                ViewBag.Message = meassage;
                ViewBag.Status = false;
            }
            return View(book);
        }

        /// <summary>
        /// for inserting a book
        /// </summary>
        /// <returns>create view of the book, returns to view of index</returns>
        public ActionResult Create()
        {
            Book book = new Book() { PublicationYear = DateTime.Now};
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book book)
        {
            try
            {
                book.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
                await svc.PostAsJsonAsync<Book>("", book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        /// <summary>
        /// for editing the details of the book
        /// </summary>
        /// <param name="bookId">specifies the book to be edited</param>
        /// <returns>view of edit, then returns to index view</returns>
        [Route("Book/Edit/{bookId}")]
        public async Task<ActionResult> Edit(int bookId)
        {
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Book/Edit/{bookId}")]
        public async Task<ActionResult> Edit(int bookId, Book book)
        {
            try
            {
                await svc.PutAsJsonAsync<Book>($"{bookId}", book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        /// <summary>
        /// for deleting the book
        /// </summary>
        /// <param name="bookId">specifies the book to be deleted</param>
        /// <returns>view of delete, then return to index view</returns>
        [Route("Book/Delete/{bookId}")]
        public async Task<ActionResult> Delete(int bookId)
        {
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Book/Delete/{bookId}")]
        public async Task<ActionResult> Delete(int bookId, Book book)
        {
            try
            {
                await svc.DeleteAsync($"{bookId}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
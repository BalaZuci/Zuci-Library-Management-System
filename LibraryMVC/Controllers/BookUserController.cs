using Azure;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;

namespace LibraryMVC.Controllers
{
    /// <summary>
    /// this Controller is used to control Book by User
    /// </summary>
    public class BookUserController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BookUser/") };
        /// <summary>
        /// for showing the list of books
        /// </summary>
        /// <returns>returns to index view</returns>
        public async Task<ActionResult> Index()
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>("");
            return View(books);
        }
        /// <summary>
        /// for showing the list of borrowed books
        /// </summary>
        /// <returns>view of the borrowed books</returns>
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
        /// <returns>view of the details</returns>
        public async Task<ActionResult> Details(int bookId)
        {
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");
            return View(book);
        }
        /// <summary>
        /// for showing the list of books written by particular author
        /// </summary>
        /// <param name="author">specifies the author name</param>
        /// <returns>view of list of books</returns>
        public async Task<ActionResult> GetByAuthor(string author)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByAuthor/{author}");
            return View(books);
        }
        /// <summary>
        /// for showing the list of books publicated on the particular year
        /// </summary>
        /// <param name="publicationYear">contains the year of publication</param>
        /// <returns>view of list of books</returns>
        public async Task<ActionResult> GetByPublicationYear(DateTime publicationYear)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByPublicationYear/{publicationYear}");
            return View(books);
        }
        /// <summary>
        /// for showing the list of books belongs to particular genre
        /// </summary>
        /// <param name="genre">specifies the genre name</param>
        /// <returns>view of list of books</returns>
        public async Task<ActionResult> GetByGenre(string genre)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByGenre/{genre}");
            return View(books);
        }
        /// <summary>
        /// for borrowing the book
        /// </summary>
        /// <param name="bookId">specifies the book to be borrowed</param>
        /// <returns>view of borrow status of book</returns>
        public async Task<ActionResult> BorrowBook(int bookId)
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");

            var response = await svc.PostAsJsonAsync<Book>($"ForBorrow/{bookId}/{borrowerId}",book);
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
        /// for returning book
        /// </summary>
        /// <param name="bookId">specifies the book to be returned</param>
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
    }
}
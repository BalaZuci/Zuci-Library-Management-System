using Azure;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;

namespace LibraryMVC.Controllers
{
    public class BookUserController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BookUser/") };
        // GET: BookUserController
        public async Task<ActionResult> Index()
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>("");
            return View(books);
        }

        public async Task<ActionResult> GetByBorrowedBooks()
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"BorrowedBooks/{borrowerId}");
            return View(books);
        }

        // GET: BookUserController/Details/5
        public async Task<ActionResult> Details(int bookId)
        {
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");
            return View(book);
        }
        
        public async Task<ActionResult> GetByAuthor(string author)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByAuthor/{author}");
            return View(books);
        }
        
        public async Task<ActionResult> GetByPublicationYear(DateTime publicationYear)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByPublicationYear/{publicationYear}");
            return View(books);
        }

        public async Task<ActionResult> GetByGenre(string genre)
        {
            List<Book> books = await svc.GetFromJsonAsync<List<Book>>($"ByGenre/{genre}");
            return View(books);
        }

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

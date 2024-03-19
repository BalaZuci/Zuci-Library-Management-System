using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace LibraryMVC.Controllers
{
    public class BookAdminController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BookAdmin/") };

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

        //Admin Methods
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

        [Route("Book/Edit/{bookId}")]
        public async Task<ActionResult> Edit(int bookId)
        {
            Book book = await svc.GetFromJsonAsync<Book>($"ByBookId/{bookId}");
            return View(book);
        }

        // POST: IndexUserController/Edit/5
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

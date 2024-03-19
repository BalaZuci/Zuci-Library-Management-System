using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TipMvcApp.Models;

namespace LibraryMVC.Controllers
{
    public class TransactionUserController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/TransactionUser/") };
        // GET: TransactionUserController
        public async Task<ActionResult> Index()
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"{borrowerId}");
            return View(transactions);
        }

        // GET: TransactionUserController/Details/5
        public async Task<ActionResult> Details(int transactionId)
        {
            Transaction transaction = await svc.GetFromJsonAsync<Transaction>($"ByTransactionId/{transactionId}");
            return View(transaction);
        }
        
        public async Task<ActionResult> GetAllBorrowTransactions(int borrowerId)
        {
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"Borrow/{ borrowerId}");
            return View(transaction);
        }
        
        public async Task<ActionResult> GetAllReturnTransactions(int borrowerId)
        {
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"Return/{borrowerId}");
            return View(transaction);
        }
        
        public async Task<ActionResult> GetTransactionsByBookId(int bookId, int borrowerId)
        {
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"Book/{bookId}/{borrowerId}");
            return View(transaction);
        }

        public ActionResult GetDates()
        {
            DateRange dateRange = new DateRange() { FromDate = DateTime.Now, ToDate = DateTime.Now };
            return View(dateRange);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDates(DateRange dateRange)
        {
            return RedirectToAction(nameof(GetTransactionsByBetweenDates), dateRange);
        }

        public async Task<ActionResult> GetTransactionsByBetweenDates(DateRange dateRange)
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"BetweenDates/{dateRange.FromDate.ToLongDateString()}/{dateRange.ToDate.ToLongDateString()}/{borrowerId}");
            return View(transaction);
        }        
    }
}

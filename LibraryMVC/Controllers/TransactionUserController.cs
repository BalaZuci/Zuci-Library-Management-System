using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TipMvcApp.Models;

namespace LibraryMVC.Controllers
{
    /// <summary>
    /// this controller is used to control the transactions by the user
    /// </summary>
    public class TransactionUserController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/TransactionUser/") };
        /// <summary>
        /// for showing the list of transactions done by particular user
        /// </summary>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> Index()
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"{borrowerId}");
            return View(transactions);
        }

        /// <summary>
        /// for showing the details of a particular transaction 
        /// </summary>
        /// <param name="transactionId">specifies the transaction</param>
        /// <returns>view of details</returns>
        public async Task<ActionResult> Details(int transactionId)
        {
            Transaction transaction = await svc.GetFromJsonAsync<Transaction>($"ByTransactionId/{transactionId}");
            return View(transaction);
        }
        /// <summary>
        /// for showing all the borrow transactions of the user
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetAllBorrowTransactions(int borrowerId)
        {
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"Borrow/{ borrowerId}");
            return View(transaction);
        }
        /// <summary>
        /// for showing all return transaction of the user
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetAllReturnTransactions(int borrowerId)
        {
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"Return/{borrowerId}");
            return View(transaction);
        }
        /// <summary>
        /// for showing transactions on particular book by the user
        /// </summary>
        /// <param name="bookId">specifies the book</param>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetTransactionsByBookId(int bookId, int borrowerId)
        {
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"Book/{bookId}/{borrowerId}");
            return View(transaction);
        }
        /// <summary>
        /// for getting from date and to date
        /// </summary>
        /// <returns>view of get dates, then to view of list of transactions</returns>
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

        /// <summary>
        /// for showing the transactions done in between from and to dates
        /// </summary>
        /// <param name="dateRange">contains from and to dates</param>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetTransactionsByBetweenDates(DateRange dateRange)
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"BetweenDates/{dateRange.FromDate.ToLongDateString()}/{dateRange.ToDate.ToLongDateString()}/{borrowerId}");
            return View(transaction);
        }        
    }
}

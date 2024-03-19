using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TipMvcApp.Models;

namespace LibraryMVC.Controllers
{
    public class TransactionAdminController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/TransactionAdmin/")};
        // GET: TransactionAdminController
        public async Task<ActionResult> Index()
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>("");
            return View(transactions);
        }
        
        public async Task<ActionResult> GetActiveTransactions()
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>("Active/");
            return View(transactions);
        }

        // GET: TransactionAdminController/Details/5
        public async Task<ActionResult> Details(int transactionId)
        {
            Transaction transaction = await svc.GetFromJsonAsync<Transaction>($"ByTransactionId/{transactionId}");
            return View(transaction);
        }

        public async Task<ActionResult> GetTransactionsByBorrowerId(int borrowerId)
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"ByBorrowerId/{borrowerId}");
            return View(transactions);
        }
        
        public async Task<ActionResult> GetAllBorrowTransactions()
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"Borrow");
            return View(transactions);
        }
        
        public async Task<ActionResult> GetAllReturnTransactions()
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"Return");
            return View(transactions);
        }

        public async Task<ActionResult> GetTransactionsByBookId(int bookId)
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"Book/{bookId}");
            return View(transactions);
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
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"BetweenDates/{dateRange.FromDate.ToLongDateString()}/{dateRange.ToDate.ToLongDateString()}");
            return View(transaction);
        }


        // GET: TransactionAdminController/Edit/5
        [Route("TransactionAdmin/Edit/{transactionId}")]
        public async Task<ActionResult> Edit(int transactionId)
        {
            Transaction transaction = await svc.GetFromJsonAsync<Transaction>($"ByTransactionId/{transactionId}");
            return View(transaction);
        }

        // POST: TransactionAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("TransactionAdmin/Edit/{transactionId}")]
        public async Task<ActionResult> Edit(int transactionId, Transaction transaction)
        {
            try
            {
                transaction.BorrowerId = 0;
                transaction.BookId = 0;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionType = "Borrow";
                await svc.PutAsJsonAsync<Transaction>($"{transactionId}", transaction);
                return RedirectToAction(nameof(GetActiveTransactions));
            }
            catch
            {
                return View();
            }
        }

        // GET: TransactionAdminController/Delete/5
        [Route("TransactionAdmin/Delete/{transactionId}")]
        public async Task<ActionResult> Delete(int transactionId)
        {
            Transaction transaction = await svc.GetFromJsonAsync<Transaction>($"ByTransactionId/{transactionId}");
            return View(transaction);
        }

        // POST: TransactionAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("TransactionAdmin/Delete/{transactionId}")]
        public async Task<ActionResult> Delete(int transactionId, Transaction transaction)
        {
            try
            {
                await svc.DeleteAsync($"{transactionId}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

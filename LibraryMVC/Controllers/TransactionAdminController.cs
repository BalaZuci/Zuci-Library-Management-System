using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TipMvcApp.Models;

namespace LibraryMVC.Controllers
{
    /// <summary>
    /// this controller controls the views of transactions by Admin
    /// </summary>
    public class TransactionAdminController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/TransactionAdmin/")};
        /// <summary>
        /// for showing the list of transactions
        /// </summary>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> Index()
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>("");
            return View(transactions);
        }
        /// <summary>
        /// for showing the active transactions(transactions which are borrowed, but not returned
        /// </summary>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetActiveTransactions()
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>("Active/");
            return View(transactions);
        }

        /// <summary>
        /// For showing the Details of a particular transaction
        /// </summary>
        /// <param name="transactionId">specifies the transaction</param>
        /// <returns>view of details of transaction</returns>
        public async Task<ActionResult> Details(int transactionId)
        {
            Transaction transaction = await svc.GetFromJsonAsync<Transaction>($"ByTransactionId/{transactionId}");
            return View(transaction);
        }
        /// <summary>
        /// for showing the transacations done by particular borrower
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetTransactionsByBorrowerId(int borrowerId)
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"ByBorrowerId/{borrowerId}");
            return View(transactions);
        }
        /// <summary>
        /// for showing all the borrow transactions 
        /// </summary>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetAllBorrowTransactions()
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"Borrow");
            return View(transactions);
        }
        /// <summary>
        /// for showing all the return transactions
        /// </summary>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetAllReturnTransactions()
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"Return");
            return View(transactions);
        }
        /// <summary>
        /// for showing transactions done on particular book
        /// </summary>
        /// <param name="bookId">specifies the book</param>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetTransactionsByBookId(int bookId)
        {
            List<Transaction> transactions = await svc.GetFromJsonAsync<List<Transaction>>($"Book/{bookId}");
            return View(transactions);
        }
        /// <summary>
        /// for getting from and to dates 
        /// </summary>
        /// <returns>view of getting dates, then returned to list of transactions</returns>
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
        /// for showing transactions between fromdate and todate
        /// </summary>
        /// <param name="dateRange">contains from and to dates</param>
        /// <returns>view of list of transactions</returns>
        public async Task<ActionResult> GetTransactionsByBetweenDates(DateRange dateRange)
        {
            List<Transaction> transaction = await svc.GetFromJsonAsync<List<Transaction>>($"BetweenDates/{dateRange.FromDate.ToLongDateString()}/{dateRange.ToDate.ToLongDateString()}");
            return View(transaction);
        }

        /// <summary>
        /// for editing the transaction details
        /// </summary>
        /// <param name="transactionId">specifies the transaction to be edited</param>
        /// <returns>view of edit, then returns to index page</returns>
        [Route("TransactionAdmin/Edit/{transactionId}")]
        public async Task<ActionResult> Edit(int transactionId)
        {
            Transaction transaction = await svc.GetFromJsonAsync<Transaction>($"ByTransactionId/{transactionId}");
            return View(transaction);
        }

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

        /// <summary>
        /// for deleting the transaction
        /// </summary>
        /// <param name="transactionId">specifies the transaction to be deleted</param>
        /// <returns>view of delete, return index page</returns>
        [Route("TransactionAdmin/Delete/{transactionId}")]
        public async Task<ActionResult> Delete(int transactionId)
        {
            Transaction transaction = await svc.GetFromJsonAsync<Transaction>($"ByTransactionId/{transactionId}");
            return View(transaction);
        }

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
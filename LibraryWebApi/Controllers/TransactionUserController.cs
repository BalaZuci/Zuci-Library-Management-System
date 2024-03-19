using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    /// <summary>
    /// this Controller is used to control Transactions By user
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionUserController : ControllerBase
    {
        /// <summary>
        /// Dependency Injection
        /// </summary>
        ITransactionRepo repo;
        public TransactionUserController(ITransactionRepo transactionRepo)
        {
            repo = transactionRepo;
        }
        /// <summary>
        /// For getting all the transactions done by particular user
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>list of transactions</returns>
        [HttpGet("{borrowerId}")]
        public async Task<ActionResult> GetAllTransactions(int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await repo.GetAllTransactionsOfUser(borrowerId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// for getting a particular transaction
        /// </summary>
        /// <param name="transactionId">specifies a transaction</param>
        /// <returns>transaction Object</returns>
        [HttpGet("ByTransactionId/{transactionId}")]
        public async Task<ActionResult> GetOneTransaction(int transactionId)
        {
            try
            {
                Transaction transaction = await repo.GetTransactionById(transactionId);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting all borrow transactions by particular user
        /// </summary>
        /// <param name="borrowerId">specifies the user</param>
        /// <returns>list of transactions</returns>
        [HttpGet("Borrow/{borrowerId}")]
        public async Task<ActionResult> GetAllBorrowTransactions(int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await repo.GetBorrowTransactionsOfUser(borrowerId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting all return transactions by particular user
        /// </summary>
        /// <param name="borrowerId">specifies the Borrower</param>
        /// <returns>list of transactions</returns>
        [HttpGet("Return/{borrowerId}")]
        public async Task<ActionResult> GetAllReturnTransactions(int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await repo.GetReturnTransactionsOfUser(borrowerId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting transactions done on particular book by particular user
        /// </summary>
        /// <param name="bookId">specifies the Book</param>
        /// <param name="borrowerId">Specifies the Borrower</param>
        /// <returns>List of transactions</returns>
        [HttpGet("Book/{bookId}/{borrowerId}")]
        public async Task<ActionResult> GetTransactionsByBookId(int bookId, int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await repo.GetTransactionsByBookIdOfUser(borrowerId,bookId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting Transactions done in between Dates by particular Borrower
        /// </summary>
        /// <param name="fromDate">From Date</param>
        /// <param name="toDate">To Date</param>
        /// <param name="borrowerId">specifies the Borrower</param>
        /// <returns></returns>
        [HttpGet("BetweenDates/{fromDate}/{toDate}/{borrowerId}")]
        public async Task<ActionResult> GetTransactionsByBetweenDates(DateTime fromDate,DateTime toDate, int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await repo.GetTransactionsBetweenDatesOfUser(borrowerId, fromDate, toDate);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

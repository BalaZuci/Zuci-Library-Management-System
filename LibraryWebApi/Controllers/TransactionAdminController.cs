using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    /// <summary>
    /// Controller to control the Transations by admin
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionAdminController : ControllerBase
    {
        /// <summary>
        /// Dependency injection
        /// </summary>
        ITransactionRepo repo;
        public TransactionAdminController(ITransactionRepo transactionRepo)
        {
            repo = transactionRepo;
        }
        /// <summary>
        /// For getting all transactions done in the library
        /// </summary>
        /// <returns>list of transactions</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllTransactions()
        {
            try
            {
                List<Transaction> transactions = await repo.GetAllTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting transaction by Id
        /// </summary>
        /// <param name="transactionId">specifies the transaction</param>
        /// <returns>Transaction object</returns>
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
        /// For getting transactions done by particular Borrower
        /// </summary>
        /// <param name="borrowerId">specifies the Borrower</param>
        /// <returns>List of transactions</returns>
        [HttpGet("ByBorrowerId/{borrowerId}")]
        public async Task<ActionResult> GetTransactionsByBorrowerId(int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await repo.GetTransactionsByBorrowerId(borrowerId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting all active transactions(which are borrowed but not returned)
        /// </summary>
        /// <returns>list of transactions</returns>
        [HttpGet("Active/")]
        public async Task<ActionResult> GetActiveTransactions()
        {
            try
            {
                List<Transaction> transactions = await repo.GetActiveTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting Borrow transactions
        /// </summary>
        /// <returns>List of transactions</returns>
        [HttpGet("Borrow")]
        public async Task<ActionResult> GetAllBorrowTransactions()
        {
            try
            {
                List<Transaction> transactions = await repo.GetBorrowTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting return transactions
        /// </summary>
        /// <returns>list of transactions</returns>
        [HttpGet("Return")]
        public async Task<ActionResult> GetAllReturnTransactions()
        {
            try
            {
                List<Transaction> transactions = await repo.GetReturnTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting transactions done on particular book
        /// </summary>
        /// <param name="bookId">specifies the book</param>
        /// <returns>List of transactions</returns>
        [HttpGet("Book/{bookId}")]
        public async Task<ActionResult> GetTransactionsByBookId(int bookId)
        {
            try
            {
                List<Transaction> transactions = await repo.GetTransactionsByBookId(bookId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For getting transactions between dates 
        /// </summary>
        /// <param name="fromDate">from date</param>
        /// <param name="toDate">to date</param>
        /// <returns>List of transactions</returns>
        [HttpGet("BetweenDates/{fromDate}/{toDate}")]
        public async Task<ActionResult> GetTransactionsByBetweenDates(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<Transaction> transactions = await repo.GetTransactionsBetweenDates(fromDate, toDate);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// for updating the transaction
        /// </summary>
        /// <param name="transactionId">specifies the Transaction to be updated</param>
        /// <param name="transaction">Contains the updated details of the transaction</param>
        /// <returns>nothing</returns>
        [HttpPut("{transactionId}")]
        public async Task<ActionResult> UpdateTransaction(int transactionId, Transaction transaction)
        {
            try
            {
                await repo.UpdateTransaction(transactionId, transaction);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For deleting a transaction
        /// </summary>
        /// <param name="transactionId">Specifies the transaction to be Deleted</param>
        /// <returns>nothing</returns>
        [HttpDelete("{transactionId}")]
        public async Task<ActionResult> DeleteTransaction(int transactionId)
        {
            try
            {
                await repo.DeleteTransaction(transactionId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
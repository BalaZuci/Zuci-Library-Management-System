using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionAdminController : ControllerBase
    {
        ITransactionRepo repo;
        public TransactionAdminController(ITransactionRepo transactionRepo)
        {
            repo = transactionRepo;
        }
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

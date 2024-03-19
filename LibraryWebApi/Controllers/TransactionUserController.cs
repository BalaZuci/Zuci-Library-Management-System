using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionUserController : ControllerBase
    {
        ITransactionRepo repo;
        public TransactionUserController(ITransactionRepo transactionRepo)
        {
            repo = transactionRepo;
        }
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

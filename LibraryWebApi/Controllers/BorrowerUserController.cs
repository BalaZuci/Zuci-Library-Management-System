using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    /// <summary>
    /// Controller to control Borrower by User
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerUserController : ControllerBase
    {
        /// <summary>
        /// Dependency Injection
        /// </summary>
        IBorrowerRepo repo;
        public BorrowerUserController(IBorrowerRepo borrowerRepo)
        {
            repo = borrowerRepo;
        }
        /// <summary>
        /// For Getting Details of the Borrower
        /// </summary>
        /// <param name="borrowerId">specifies the Borrower</param>
        /// <returns>Borrower Object</returns>
        [HttpGet("{borrowerId}")]
        public async Task<ActionResult> GetById(int borrowerId)
        {
            try
            {
                Borrower borrower = await repo.GetBorrowerById(borrowerId);
                return Ok(borrower);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// for getting Borrower Object for log in and register page
        /// </summary>
        /// <param name="email">specifies the Borrower</param>
        /// <returns>Borrower Object</returns>
        [HttpGet("ByEmail/{email}")]
        public async Task<ActionResult> GetBorrowerByEmail(string email)
        {
            try
            {
                Borrower borrower = await repo.GetBorrowerByEmail(email);
                return Ok(borrower);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// for registering the new user
        /// used in registering page
        /// </summary>
        /// <param name="borrower">Borrower object to be inserted</param>
        /// <returns>nothing</returns>
        [HttpPost]
        public async Task<ActionResult> InsertBorrower(Borrower borrower)
        {
            try
            {
                await repo.InsertBorrower(borrower);
                return Created($"api/BorrowerUser/{borrower.BorrowerId}", borrower);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For updating own details
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <param name="borrower">contains updated details of borrower</param>
        /// <returns>nothing</returns>
        [HttpPut("ForUpdate/{borrowerId}")]
        public async Task<ActionResult> UpdateBorrower(int borrowerId, Borrower borrower)
        {
            try
            {
                await repo.UpdateBorrower(borrowerId, borrower);
                return Ok(borrower);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    /// <summary>
    /// Controller to Control Borrower by Admin
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerAdminController : ControllerBase
    {
        /// <summary>
        /// Dependency Injection
        /// </summary>
        IBorrowerRepo repo;
        public BorrowerAdminController(IBorrowerRepo borrowerRepo)
        {
            repo = borrowerRepo;
        }

        /// <summary>
        /// For getting All the Borrowers
        /// </summary>
        /// <returns>list of borrowers</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllBorrowers()
        {
            List<Borrower> borrowers = await repo.GetAllBorrowers();
            return Ok(borrowers);
        }
        /// <summary>
        /// Getting Borrower by Id
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>Borrower Object</returns>
        [HttpGet("{borrowerId}")]
        public async Task<ActionResult> GetBorrowerById(int borrowerId)
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
        /// Getting borrower by email
        /// using for logging in and registering pages
        /// </summary>
        /// <param name="email">specifies the borrower</param>
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
        /// for getting Borrowers of particular type(Admin or User)
        /// </summary>
        /// <param name="borrowerType">specifies Admin or User</param>
        /// <returns>List of borrowers</returns>
        [HttpGet("ByBorrowerType/{borrowerType}")]
        public async Task<ActionResult> GetBorrowersByType(string borrowerType)
        {
            try
            {
                List<Borrower> borrowers = await repo.GetBorrowerByBorrowerType(borrowerType);
                return Ok(borrowers);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// for inserting new borrower
        /// </summary>
        /// <param name="borrower">Contains the details of the new user</param>
        /// <returns>nothing</returns>
        [HttpPost]
        public async Task<ActionResult> InsertBorrower(Borrower borrower)
        {
            try
            {
                await repo.InsertBorrower(borrower);
                return Created($"api/BorrowerAdmin/{borrower.BorrowerId}", borrower);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// For updating the details of the existing user
        /// </summary>
        /// <param name="borrowerId">specifies the borrower details to be edited</param>
        /// <param name="borrower">contains updated details</param>
        /// <returns>nothing</returns>
        [HttpPut("{borrowerId}")]
        public async Task<ActionResult> UpdateBorrower(int borrowerId, Borrower borrower)
        {
            try
            {
                await repo.UpdateBorrowerByAdmin(borrowerId, borrower);
                return Ok(borrower);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// for Deleting the Borrower
        /// </summary>
        /// <param name="borrowerId">specifies the Borrower to be deleted</param>
        /// <returns>nothing</returns>
        [HttpDelete("{borrowerId}")]
        public async Task<ActionResult> DeleteBorrower(int borrowerId)
        {
            try
            {
                await repo.DeleteBorrower(borrowerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

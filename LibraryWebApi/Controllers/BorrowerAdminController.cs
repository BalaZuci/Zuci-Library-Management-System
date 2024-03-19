using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerAdminController : ControllerBase
    {
        IBorrowerRepo repo;
        public BorrowerAdminController(IBorrowerRepo borrowerRepo)
        {
            repo = borrowerRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBorrowers()
        {
            List<Borrower> borrowers = await repo.GetAllBorrowers();
            return Ok(borrowers);
        }

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

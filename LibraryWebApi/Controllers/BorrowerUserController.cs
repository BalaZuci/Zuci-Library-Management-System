using Library.Models;
using Library.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerUserController : ControllerBase
    {
        IBorrowerRepo repo;
        public BorrowerUserController(IBorrowerRepo borrowerRepo)
        {
            repo = borrowerRepo;
        }
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

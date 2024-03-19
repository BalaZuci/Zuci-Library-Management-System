using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    /// <summary>
    /// this controller is used to control own details of the borrower
    /// </summary>
    public class BorrowerUserController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BorrowerUser/") };
        /// <summary>
        /// for showing own details of the borrower
        /// </summary>
        /// <returns>view of details of the user</returns>
        public async Task<ActionResult> Details()
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            Borrower borrower = await svc.GetFromJsonAsync<Borrower>($"{borrowerId}");
            return View(borrower);
        }
        /// <summary>
        /// for editing own details of the borrower
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>view of editing details, then to details page</returns>
        [Route("BorrowerUser/Edit/{borrowerId}")]
        public async Task<ActionResult> Edit(int borrowerId)
        {
            Borrower borrower = await svc.GetFromJsonAsync<Borrower>($"{borrowerId}");
            return View(borrower);
        }

        // POST: BorrowerUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("BorrowerUser/Edit/{borrowerId}")]
        public async Task<ActionResult> Edit(int borrowerId, Borrower borrower)
        {
            try
            {
                borrower.BorrowerType = HttpContext.Session.GetString("Type");
                borrower.EmpId = "123";
                await svc.PutAsJsonAsync<Borrower>($"ForUpdate/{borrowerId}", borrower);
                return RedirectToAction(nameof(Details));
            }
            catch
            {
                return View();
            }
        }
    }
}

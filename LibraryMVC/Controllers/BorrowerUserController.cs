using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class BorrowerUserController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BorrowerUser/") };
        // GET: BorrowerUserController/Details/5
        public async Task<ActionResult> Details()
        {
            int borrowerId = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
            Borrower borrower = await svc.GetFromJsonAsync<Borrower>($"{borrowerId}");
            return View(borrower);
        }

        // GET: BorrowerUserController/Edit/5
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

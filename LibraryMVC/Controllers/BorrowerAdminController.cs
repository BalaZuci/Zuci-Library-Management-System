using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class BorrowerAdminController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BorrowerAdmin/") };
       
        public async Task<ActionResult> Index()
        {
            List<Borrower> borrowers = await svc.GetFromJsonAsync<List<Borrower>>("");
            return View(borrowers);
        }

        public async Task<ActionResult> Details(int borrowerId)
        {
            Borrower borrower = await svc.GetFromJsonAsync<Borrower>($"{borrowerId}");
            return View(borrower);
        }

        public async Task<ActionResult> GetBorrowersByType(string borrowerType)
        {
            List<Borrower> borrowers = await svc.GetFromJsonAsync<List<Borrower>>($"ByBorrowerType/{borrowerType}");
            return View(borrowers);
        }

        public ActionResult Create()
        {
            Borrower borrower = new Borrower();
            return View(borrower);
        }

        // POST: BorrowerAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Borrower borrower)
        {
            try
            {
                HttpResponseMessage response = await svc.GetAsync($"ByEmail/{borrower.Email}");
                if (response.IsSuccessStatusCode)
                {
                    ViewData["ErrorMessage"] = "This Email is already exists. Please Login.";
                    return View("Create", borrower);
                }
                else
                {
                    borrower.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("BorrowerId"));
                    await svc.PostAsJsonAsync<Borrower>("", borrower);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        [Route("BorrowerAdmin/Edit/{borrowerId}")]
        public async Task<ActionResult> Edit(int borrowerId)
        {
            Borrower borrower = await svc.GetFromJsonAsync<Borrower>($"{borrowerId}");
            return View(borrower);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("BorrowerAdmin/Edit/{borrowerId}")]
        public async Task<ActionResult> Edit(int borrowerId, Borrower borrower)
        {
            try
            {
                borrower.CreatedBy = 1;
                borrower.Password = "123";
                borrower.EmpId = "456";
                await svc.PutAsJsonAsync<Borrower>($"{borrowerId}", borrower);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Route("BorrowerAdmin/Delete/{borrowerId}")]
        public async Task<ActionResult> Delete(int borrowerId)
        {
            Borrower borrower = await svc.GetFromJsonAsync<Borrower>($"{borrowerId}");
            return View(borrower);
        }

        // POST: BorrowerAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("BorrowerAdmin/Delete/{borrowerId}")]
        public async Task<ActionResult> Delete(int borrowerId, Borrower borrower)
        {
            try
            {
                await svc.DeleteAsync($"{borrowerId}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

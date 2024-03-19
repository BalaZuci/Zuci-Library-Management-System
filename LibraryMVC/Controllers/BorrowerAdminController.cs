using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    /// <summary>
    /// this controller is used to control Borrowers by Admin
    /// </summary>
    public class BorrowerAdminController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BorrowerAdmin/") };
       /// <summary>
       /// for showing the list of borrowers
       /// </summary>
       /// <returns>view of list of borrowers</returns>
        public async Task<ActionResult> Index()
        {
            List<Borrower> borrowers = await svc.GetFromJsonAsync<List<Borrower>>("");
            return View(borrowers);
        }
        /// <summary>
        /// for showing the details of the particular borrower
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>view of details of the borrower</returns>
        public async Task<ActionResult> Details(int borrowerId)
        {
            Borrower borrower = await svc.GetFromJsonAsync<Borrower>($"{borrowerId}");
            return View(borrower);
        }
        /// <summary>
        /// for showing the borrowers of same type(Admin or User)
        /// </summary>
        /// <param name="borrowerType">specifies the type of Borrower</param>
        /// <returns>view of list of borrowers</returns>
        public async Task<ActionResult> GetBorrowersByType(string borrowerType)
        {
            List<Borrower> borrowers = await svc.GetFromJsonAsync<List<Borrower>>($"ByBorrowerType/{borrowerType}");
            return View(borrowers);
        }
        /// <summary>
        /// for inserting a new borrower
        /// </summary>
        /// <returns>view of the create page, then returns to index page</returns>
        public ActionResult Create()
        {
            Borrower borrower = new Borrower();
            return View(borrower);
        }

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

        /// <summary>
        /// for editing the details of the existing borrower
        /// </summary>
        /// <param name="borrowerId">specifies the Borrower</param>
        /// <returns>view of editing page, then to index page</returns>
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
        /// <summary>
        /// for deleting the borrower
        /// </summary>
        /// <param name="borrowerId">specifies the Borrower</param>
        /// <returns>view of Delete page, then returns to Index page</returns>
        [Route("BorrowerAdmin/Delete/{borrowerId}")]
        public async Task<ActionResult> Delete(int borrowerId)
        {
            Borrower borrower = await svc.GetFromJsonAsync<Borrower>($"{borrowerId}");
            return View(borrower);
        }
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
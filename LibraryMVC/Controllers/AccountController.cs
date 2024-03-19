using Library.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace LibraryMVC.Controllers
{
    public class AccountController : Controller
    {
        static HttpClient svc = new HttpClient() { BaseAddress = new Uri("http://localhost:5273/api/BorrowerUser/") };
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Verify(string email, string password)
        {
            HttpResponseMessage response = await svc.GetAsync($"ByEmail/{email}");
            if (response.IsSuccessStatusCode)
            {
                Borrower borrower = await response.Content.ReadFromJsonAsync<Borrower>();
                if (borrower.Email == email && borrower.Password == password)
                {
                    HttpContext.Session.SetString("Email", email);
                    HttpContext.Session.SetString("BorrowerId", Convert.ToString(borrower.BorrowerId));
                    HttpContext.Session.SetString("Name", borrower.Name);
                    HttpContext.Session.SetString("Type", borrower.BorrowerType);
                    HttpContext.Session.SetString("IsLogin", "true");
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Please Enter Correct Password.";
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please Enter Correct Email or Sign Up for New Account.";
                return RedirectToAction("Login", "Account");
            }
        }



        // GET: AccountController/Create
        public ActionResult Register()
        {
            Borrower borrower = new Borrower();
            return View(borrower);
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Borrower borrower)
        {
            try
            {
                HttpResponseMessage response = await svc.GetAsync($"ByEmail/{borrower.Email}");
                if (response.IsSuccessStatusCode)
                {
                    ViewData["ErrorMessage"] = "This Email is already exists. Please Login.";
                    return View("Register", borrower);
                }
                else
                {
                    borrower.BorrowerType = "User";
                    await svc.PostAsJsonAsync<Borrower>("", borrower);
                    return RedirectToAction(nameof(Login));
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); 
        }
    }
}

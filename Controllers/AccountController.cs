using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryApp.Models;
using LibraryApp.Models.AccountViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;

namespace LibraryApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public AccountController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                View(await _userManager.Users.ToListAsync());
            else if (roles.Contains("Librarian"))
                View(await _userManager.GetUsersInRoleAsync("Reader"));
            return View();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Roles"] = _context.Roles.ToList();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var lastRFID = (await _userManager.Users.LastOrDefaultAsync()).RFID.GetValueOrDefault();
                lastRFID = lastRFID + 1;
                var role = User.IsInRole("Admin") ? model.Role : "Reader";
                var user = new User { UserName = model.Email, Email = model.Email ,
                                        Firstname = model.Firstname, Lastname = model.Lastname, LoanLimit = model.LoanLimit,
                                        PersonalNumber = model.PersonalNumber, RFID = lastRFID };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, role);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(3, "Created a new account with password.");
                        return RedirectToAction("Index");
                    }
                    else
                        await _userManager.RemoveFromRoleAsync(user, role);
                }
                AddErrors(result);
            }
            ViewData["Roles"] = _context.Roles.ToList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditViewModel regModel = new EditViewModel() 
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                PersonalNumber = user.PersonalNumber,
                LoanLimit = user.LoanLimit,
            };
            ViewData["Roles"] = _context.Roles.ToList();
            return View(regModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> EditConfirm(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = User.IsInRole("Admin") || User.IsInRole("Librarian") ? model.Role : "Reader";
                var user = await _userManager.FindByIdAsync(model.Id);
                user.Firstname = user.Firstname;
                user.Lastname = user.Lastname;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.LoanLimit = model.LoanLimit;
                user.PersonalNumber = model.PersonalNumber;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.RemoveFromRolesAsync(user,  await _userManager.GetRolesAsync(user));

                    if (roleResult.Succeeded)
                    {
                        roleResult = await _userManager.AddToRoleAsync(user, role);
                        if (roleResult.Succeeded)
                        {
                            _logger.LogInformation(3, "User created a new account with password.");
                            return RedirectToAction("Index");
                        }
                    }
                    AddErrors(roleResult);
                }
                AddErrors(result);
            }
            ViewData["Roles"] = _context.Roles.ToList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Role"] = (await _userManager.GetRolesAsync(user)).First();
            user.Loans = await _context.Loans.Where( l => l.UserId == user.Id).Include(b => b.Book).ToListAsync();
            return View(user);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}

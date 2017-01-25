using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Models.HomeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        
        public HomeController(
            ApplicationDbContext context,
            UserManager<User> userManager
            )
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Loan> loans = new List<Loan>();
            if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var result = await _userManager.GetUserAsync(HttpContext.User);
                loans = result.Loans;
            }

            var indexModel = new IndexViewModel()
            {
                LastBooks = await _context.Books.OrderByDescending( b => b.Id ).Take(4).ToListAsync(),
                LastLoans = loans
            };
            return View(indexModel);
        }

        public async Task<IActionResult> Search(int SearchId)
        {
            var user = await _userManager.Users.FirstAsync( b => b.RFID == SearchId);
            var searchView = new SearchViewModel()
            {
                Book = await _context.Books.FirstAsync( b => b.Id == SearchId),
                User = await _userManager.Users.FirstAsync( b => b.RFID == SearchId)
            };
            return View(searchView);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Models.HomeViewModels;
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
                loans = await _context.Loans.Where( l => l.UserId == result.Id).Take(4).ToListAsync();
            }

            var indexModel = new IndexViewModel()
            {
                LastBooks = await _context.Books.OrderByDescending( b => b.Id ).Take(4).ToListAsync(),
                LastLoans = loans
            };
            return View(indexModel);
        }

        public async Task<IActionResult> Search(string SearchString)
        {
            if (SearchString.Length == 10)
            {
                SearchString = SearchString.Replace("0","");
                int id;
                if (int.TryParse(SearchString, out id))
                {
                    var searchView = new SearchViewModel()
                    {
                        Book = new List<Book>() { await _context.Books.FirstOrDefaultAsync( b => b.Id == id) },
                        User = await _userManager.Users.FirstOrDefaultAsync( b => b.RFID == id),
                        isForBookList = false
                    };
                    return View(searchView);
                }
            }

            var searchViewBook = new SearchViewModel()
            {
                Book = await _context.Books.Where( b => (b.Title.Contains(SearchString) 
                                            || b.Anotation.Contains(SearchString) || b.Genre.Contains(SearchString)
                                            || b.Author.Contains(SearchString))).ToListAsync(),
                isForBookList = true
            };
            return View(searchViewBook);

        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

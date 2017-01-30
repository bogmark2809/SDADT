using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Models.LoanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Index()
        {
            var loans = _context.Loans.Include( w => w.User);
            return View(await loans.Include(b => b.Book).ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [Authorize(Roles = "Admin,Librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.Include( u => u.Loans).FirstOrDefaultAsync( b => b.RFID == model.RFID);
                var book = await _context.Books.FirstOrDefaultAsync( b => b.Id == model.Code);
                if (book == null || user == null)
                    ModelState.AddModelError(string.Empty, string.Format("There is no user or book with such ID"));
                if (ModelState.ErrorCount > 0)
                    return View(model);
                if ((await _context.Loans.FirstOrDefaultAsync( l => (l.UserId == user.Id && l.BookId == book.Id ))) != null)
                    ModelState.AddModelError(string.Empty, string.Format("Exist user or book with same Id"));
                if (user.LoanLimit <= user.Loans.Count)
                    ModelState.AddModelError(string.Empty, string.Format("The user uses the entire loan limit"));
                if (!book.isAvailable)
                    ModelState.AddModelError(string.Empty, string.Format("The book is not available"));
                if (ModelState.ErrorCount > 0)
                    return View(model);
                var loan = new Loan()
                {
                    Book = book,
                    User= user,
                    LoanDate = DateTime.Now,
                    ReturnDate = model.ReturnDate
                };
                book.Quantity = book.Quantity - 1;
                if (book.Quantity == 0)
                    book.isAvailable = false;
                _context.Update(book);    
                await _context.AddAsync(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.SingleOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }
            var editModel = new EditViewModel()
            {
                id = loan.Id,
                ReturnDate = loan.ReturnDate,
                isReturned = loan.isReturned
            };
            return View(editModel);
        }

        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = "Admin,Librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirm(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var loan = await _context.Loans.FirstOrDefaultAsync(l => l.Id == model.id);
                loan.ReturnDate = model.ReturnDate;
                loan.isReturned = model.isReturned;
                if (loan.isReturned)
                {
                    var book = await _context.Books.FirstOrDefaultAsync(l => l.Id == loan.BookId); 
                    book.Quantity = book.Quantity + 1;
                    if (book.Quantity == 1)
                        book.isAvailable = true;
                }
                _context.Update(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.SingleOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FirstAsync( l => l.Id == id);
            var book = await _context.Books.FirstAsync( b => b.Id == loan.BookId); 
            _context.Loans.Remove(loan);
            book.Quantity = book.Quantity + 1;
            if (book.Quantity == 1)
                book.isAvailable = true;
            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
using System;
using System.Collections.Generic;
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
                var user = await _context.Users.FirstAsync( b => b.RFID == model.RFID);
                var book = await _context.Books.FirstAsync( b => b.Id == model.Code);
                var loan = new Loan()
                {
                    Book = book,
                    User= user,
                    LoanDate = DateTime.Now,
                    ReturnDate = model.ReturnDate
                };
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

            return View(loan);
        }

        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = "Admin,Librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirm(Loan model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
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
            var loan = await _context.Loans.SingleOrDefaultAsync(m => m.Id == id);
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
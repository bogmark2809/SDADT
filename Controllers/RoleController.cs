using System.Threading.Tasks;
using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<Role> _roleManager;

        public RoleController(ApplicationDbContext context, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(Role model)
        {
            if (ModelState.IsValid)
            {
                var role = new Role(model.Name);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                //AddErrors(result);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.SingleOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditConfirm(Role model)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.SingleOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(m => m.Id == id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
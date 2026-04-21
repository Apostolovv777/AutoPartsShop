using AutoPartsShop.Infrastructure.Data;
using AutoPartsShop.Infrastructure.Data.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsShop.Controllers
{
    [Authorize]
    public class FavouritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavouritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var favourites = await _context.Favourites
                .Where(f => f.UserId == user.Id)
                .Include(f => f.Product)
                .ToListAsync();

            return View(favourites);
        }

        public async Task<IActionResult> Add(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var exists = await _context.Favourites
                .FirstOrDefaultAsync(f => f.ProductId == id && f.UserId == user.Id);

            if (exists == null)
            {
                _context.Favourites.Add(new Favourite
                {
                    ProductId = id,
                    UserId = user.Id
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var favourite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.ProductId == id && f.UserId == user.Id);

            if (favourite != null)
            {
                _context.Favourites.Remove(favourite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
using AutoPartsShop.Infrastructure.Data;
using AutoPartsShop.Infrastructure.Data.Entities;
using AutoPartsShop.Models.Cart;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

namespace AutoPartsShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private List<CartItemVM> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("cart");

            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItemVM>();
            }

            return JsonSerializer.Deserialize<List<CartItemVM>>(cartJson) ?? new List<CartItemVM>();
        }

        private void SaveCart(List<CartItemVM> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("cart", cartJson);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult AddToCart(int id, string name, decimal price, string imageUrl)
        {
            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(x => x.ProductId == id);

            if (existingItem == null)
            {
                cart.Add(new CartItemVM
                {
                    ProductId = id,
                    ProductName = name,
                    Price = price,
                    ImageUrl = imageUrl,
                    Quantity = 1
                });
            }
            else
            {
                existingItem.Quantity++;
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Increase(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrease(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("cart");
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);

            foreach (var item in cart)
            {
                var order = new Order
                {
                    ProductId = item.ProductId,
                    UserId = user.Id,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderDate = DateTime.Now
                };

                _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("cart");

            return RedirectToAction("MyOrders", "Order");
        }
    }
}
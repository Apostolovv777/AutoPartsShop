using AutoPartsShop.Infrastructure.Data.Entities;
using AutoPartsShop.Models.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoPartsShop.Controllers
{
    public class ClientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        // GET: ClientController
        public async Task<IActionResult> Index()
        {
            var allUsers = this._userManager.Users
                .Select(u => new ClientIndexVM
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Address = u.Address,
                    Email = u.Email,
                })
                .ToList();

            // Id на всички администратори
            var adminIds = (await _userManager.GetUsersInRoleAsync("Administrator"))
                .Select(a => a.Id).ToList();

            // Ако потребителят е в списъка, то IsAdmin става true
            foreach (var user in allUsers)
            {
                user.IsAdmin = adminIds.Contains(user.Id);
            }

            // Вадим само клиентите без администратора и ги сортираме по username
            var users = allUsers.Where(x => x.IsAdmin == false)
                .OrderBy(x => x.UserName).ToList();

            // връщаме списъка
            return this.View(users);
        }

        // GET: ClientController/Delete/5
        public ActionResult Delete(string id)
        {
            var user = this._userManager.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ClientDeleteVM userToDelete = new ClientDeleteVM()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email,
                UserName = user.UserName
            };

            return View(userToDelete);
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ClientDeleteVM bidingModel)
        {
            string id = bidingModel.Id;
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Success");
            }

            return NotFound();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}

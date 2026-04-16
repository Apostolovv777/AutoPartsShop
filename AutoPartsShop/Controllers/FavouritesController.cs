using AutoPartssShop.Core.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

[Authorize]
public class FavouritesController : Controller
{
    private readonly IFavouriteService favouriteService;

    public FavouritesController(IFavouriteService favouriteService)
    {
        this.favouriteService = favouriteService;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<IActionResult> Index()
    {
        var favs = await favouriteService.GetUserFavourites(GetUserId());
        return View(favs);
    }

    public async Task<IActionResult> Add(int id)
    {
        await favouriteService.AddAsync(GetUserId(), id);
        return RedirectToAction("Index", "Product");
    }

    public async Task<IActionResult> Remove(int id)
    {
        await favouriteService.RemoveAsync(GetUserId(), id);
        return RedirectToAction(nameof(Index));
    }
}
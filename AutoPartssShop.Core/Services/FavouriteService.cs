using AutoPartsShop.Infrastructure.Data;
using AutoPartsShop.Infrastructure.Data.Entities;

using AutoPartssShop.Core.Contracts;

using Microsoft.EntityFrameworkCore;

public class FavouriteService : IFavouriteService
{
    private readonly ApplicationDbContext context;

    public FavouriteService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(string userId, int productId)
    {
        if (!context.Favourites.Any(f => f.UserId == userId && f.ProductId == productId))
        {
            var fav = new Favourite
            {
                UserId = userId,
                ProductId = productId
            };

            context.Favourites.Add(fav);
            await context.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(string userId, int productId)
    {
        var fav = await context.Favourites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        if (fav != null)
        {
            context.Favourites.Remove(fav);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Favourite>> GetUserFavourites(string userId)
    {
        return await context.Favourites
            .Where(f => f.UserId == userId)
            .Select(f => new FavouriteVM
            {
                ProductId = f.ProductId,
                ProductName = f.Product.ProductName,
                ImageUrl = f.Product.Picture,
                Price = f.Product.Price
            })
            .ToListAsync();
    }

    Task<IEnumerable<FavouriteVM>> IFavouriteService.GetUserFavourites(string userId)
    {
        throw new NotImplementedException();
    }
}
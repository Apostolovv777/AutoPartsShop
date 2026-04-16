using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPartssShop.Core.Contracts
{
    public interface IFavouriteService
    {
        Task AddAsync(string userId, int productId);
        Task RemoveAsync(string userId, int productId);
        Task<IEnumerable<FavouriteVM>> GetUserFavourites(string userId);
    }
}

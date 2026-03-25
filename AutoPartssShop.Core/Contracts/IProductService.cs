using AutoPartsShop.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPartssShop.Core.Contracts
{
    public interface IProductService
    {
        bool Create(string name, int brandId, int categoryId, string picture, string description, int quantity, decimal price, decimal discount);

        bool Update(int productId, string name, int brandId, int categoryId, string picture, string description, int quantity, decimal price, decimal discount);

        List<Product> GetProducts();

        Product GetProductById(int productId);

        bool RemoveById(int dogproductId);

        List<Product> GetProducts(string searchStringCategoryName, string searchStringBrandName);
    }
}
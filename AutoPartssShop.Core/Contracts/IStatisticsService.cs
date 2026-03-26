using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPartssShop.Core.Contracts
{
    public interface IStatisticService
    {
        int CountProducts();
        int CountClients();
        int CountOrders();
        decimal SumOrders();
    }
}

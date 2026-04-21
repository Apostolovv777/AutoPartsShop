using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPartsShop.Infrastructure.Data.Entities
{
    public class Favourite
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
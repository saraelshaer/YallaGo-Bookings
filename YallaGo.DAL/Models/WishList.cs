using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YallaGo.DAL.Models
{
    public class WishList
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<WishListItems> WishListItems { get; set; } = new List<WishListItems>();
    }
}

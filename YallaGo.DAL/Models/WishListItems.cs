using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YallaGo.DAL.Models
{
    public class WishListItems
    {
        public int Id { get; set; }

        [ForeignKey("WishList")]
        public int WishListId { get; set; }
        public virtual WishList WishList { get; set; }

        [ForeignKey("Tour")]
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }
    }
}

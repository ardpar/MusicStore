using MainMusicStore.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainMusicStore.Models.ViewModels
{
    public class ShoppingCardVM
    {
        public IEnumerable<ShoppingCard> ListCard { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}

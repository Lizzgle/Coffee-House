using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Entities
{
    public class Cart : Base
    {
        public Cart(long id) {
            Id = id;
        }

        public User User { get; set; }

        public List<Drink> Drinks { get; set; } = new List<Drink>();
        public List<Dessert> Desserts { get; set; } = new List<Dessert>();


    }
}

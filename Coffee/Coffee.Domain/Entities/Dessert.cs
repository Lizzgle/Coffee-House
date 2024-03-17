using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Entities
{
    public class Dessert : Base
    {
        public string Name { get; set; }
        public int Calories { get; set; }

        public float Price { get; set; }

        public List<Order>? Orders { get; set; } = new List<Order>();
        public List<Cart>? Carts { get; set; } = new List<Cart>();
    }
}

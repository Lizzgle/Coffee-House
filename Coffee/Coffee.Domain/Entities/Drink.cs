using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Entities
{
    public class Drink : Base
    {
        public string Name { get; set; }

        public Size Size { get; set; }

        public float Price { get; set; }

        public Recipe? Recipe { get; set; }
        public Category? Category { get; set; }

        public List<Order>? Orders { get; set; }
        public List<Cart>? Carts { get; set; }
    }

    public enum Size
    {
        S,
        M,
        L
    }
}

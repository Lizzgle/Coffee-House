using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Entities
{
    public class Order : Base
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        public List<Drink> Drinks { get; set; }
        public List<Dessert> Desserts { get; set; }
        public Drink Drink { get; set; }
        public Dessert Dessert { get; set; }
    }
}

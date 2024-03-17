using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Entities
{
    public class Category : Base
    {
        public string Name { get; set; }

        public List<Drink> Drinks { get; set; } = new List<Drink>();
    }
}

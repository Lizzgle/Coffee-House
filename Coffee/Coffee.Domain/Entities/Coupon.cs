using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Entities
{
    public class Coupon : Base
    {
        public int Discount { get; set; }

        public DateTime DateOfEnd { get; set; }

        public List<User> Users { get; set; }
    }
}

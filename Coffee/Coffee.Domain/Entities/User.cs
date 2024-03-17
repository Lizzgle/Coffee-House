using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Entities
{
    public class User : Base
    {
        //public User() {
        //    cartId = Id;
        //}

        public string Login { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Role Role { get; set; } = Role.NoName;
        public Cart Cart { get; set; }

        public long CartId { get; set; }
        public List<Feedback> Feedbacks { get; set; } = new();
        public List<Order> Orders { get; set; } = new();


    }
    public enum Role
    {
        NoName,
        Moderator,
        Admin,
        User
    }
}

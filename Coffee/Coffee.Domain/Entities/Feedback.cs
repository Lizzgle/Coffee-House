using Coffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Entities
{
    public class Feedback : Base
    {
        public DateTime Date { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }
    }
}

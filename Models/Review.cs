using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int? Rating { get; set; }
        public DateTime CTime { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
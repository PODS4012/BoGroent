using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Models
{
    public class Leases
    {
        public int Id { get; set; }

        public string CarId { get; set; }
        public string UserID { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Paid { get; set; }
    }
}

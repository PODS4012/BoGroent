using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Models
{
    public class Lease
    {
        public int Id { get; set; }

        [Required, Display(Name = "Car Id")]
        public int CarPlateId { get; set; }

        public string CarBrand { get; set; }
        public string CarColor { get; set; }

        [DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        public decimal CarRentPrice { get; set; }

        [Required, Display(Name = "User Id")]
        public string UserUserName { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "The Start Date is required")]
        [DataType(DataType.Date), Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "The End Date is required")]
        [DataType(DataType.Date), Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Total Rent Price"), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get { return (EndDate.Date - StartDate.Date).Days * CarRentPrice; } }

        public string Paid { get; set; }

        public Lease() { }

        public Lease(Car car, AppUser user)
        {
            CarBrand = car.Brand;
            CarColor = car.Color;
            CarRentPrice = car.RentPrice;
            UserId = user.Id;
            UserName = user.Name;
        }
    }
}

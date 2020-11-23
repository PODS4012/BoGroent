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
        [Display(Name = "Lease Id")]
        public int Id { get; set; }

        [Required, Display(Name = "Car Id")]
        public int CarId { get; set; }

        [Display(Name = "Brand")]
        public string CarBrand { get; set; }

        [Display(Name = "Car Color")]
        public string CarColor { get; set; }

        [DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Price")]
        public decimal CarRentPrice { get; set; }

        [Required, Display(Name = "User Name")]
        public string UserUserName { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The Start Date is required")]
        [DataType(DataType.Date), Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "The End Date is required")]
        [DataType(DataType.Date), Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Total Price"), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get { return (EndDate.Date - StartDate.Date).Days * CarRentPrice; } }

        public string Payment { get; set; }
    }
}

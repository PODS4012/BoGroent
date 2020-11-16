using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 2)]
        public string Brand { get; set; }

        [Required, Range(10, 1000), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Rent Price")]
        public decimal RentPrice { get; set; }

        [Column(TypeName = "nvarchar(max)"), Display(Name ="Picture")]
        public string PicFileName { get; set; }
    }
}

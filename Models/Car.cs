using BoGroent.Infrastructure;
using Microsoft.AspNetCore.Http;
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

        [Required, StringLength(20, MinimumLength = 3)]
        public string Color { get; set; }

        [Display(Name = "Plate No.")]
        public string IdPlate { get { return "Bo Grønt " + Id.ToString(); } }

        [Required, Range(10, 1000), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Rent Price")]
        public decimal RentPrice { get; set; }

        [Column(TypeName = "nvarchar(max)"), Display(Name ="Car Image")]
        public string Image { get; set; }

        [NotMapped, FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}

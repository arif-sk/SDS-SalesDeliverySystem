using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Application.Models
{
    public class OrderCreateRequestViewModel
    {
        [Required]
        public string Client { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string ProductSpecification { get; set; }
        [Required]
        public double Budget { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public string WarrantyInformation { get; set; }

    }

    public class OrderUpdateRequestViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Client { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string ProductSpecification { get; set; }
        [Required]
        public double Budget { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public string WarrantyInformation { get; set; }

    }
}

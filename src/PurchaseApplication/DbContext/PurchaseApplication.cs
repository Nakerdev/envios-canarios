using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CanaryDeliveries.PurchaseApplication.DbContext
{
    public sealed class PurchaseApplication
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        public List<Product> Products { get; set; }
        
        [Required]
        public Client Client { get; set; }
        
        [MaxLength(1000)]
        public string AdditionalInformation { get; set; }

        public DateTime CreationDateTime { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace CanaryDeliveries.PurchaseApplication.DbContext
{
    public sealed class Product
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Link { get; set; }
        
        [Required]
        public int Units { get; set; }
        
        [MaxLength(1000)]
        public string AdditionalInformation { get; set; }
        
        [MaxLength(50)]
        public string PromotionCode { get; set; }
    }
}
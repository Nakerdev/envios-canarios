using System.ComponentModel.DataAnnotations;

namespace CanaryDeliveries.PurchaseApplication.DbContext
{
    public sealed class Client
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(16)]
        public string PhoneNumber { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
    }
}
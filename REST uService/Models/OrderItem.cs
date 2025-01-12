using System.ComponentModel.DataAnnotations;

namespace REST_uService.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal PricePerUnit { get; set; }
    }
}

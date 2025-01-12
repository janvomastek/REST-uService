using System.ComponentModel.DataAnnotations;

namespace REST_uService.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public OrderStatus Status { get; set; } = OrderStatus.New;
        public List<OrderItem> Items { get; set; }
    }
}

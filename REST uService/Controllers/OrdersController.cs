using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_uService.Data;
using REST_uService.Services;

namespace REST_uService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PaymentQueue _paymentQueue;

        public OrdersController(AppDbContext context, PaymentQueue paymentQueue)
        {
            _context = context;
            _paymentQueue = paymentQueue;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Models.Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders.Include(o => o.Items).ToListAsync();
            return Ok(orders);
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> ProcessPayment(int id, [FromBody] bool isPaid)
        {
            // Enqueue the payment processing request
            await _paymentQueue.EnqueuePaymentAsync(id, isPaid);
            return Accepted(new { Message = $"Payment processing for Order ID {id} has been queued." });
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}

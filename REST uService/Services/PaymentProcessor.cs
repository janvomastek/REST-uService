using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using REST_uService.Data;
using REST_uService.Models;

namespace REST_uService.Services
{
    public class PaymentProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PaymentQueue _paymentQueue;

        public PaymentProcessor(IServiceProvider serviceProvider, PaymentQueue paymentQueue)
        {
            _serviceProvider = serviceProvider;
            _paymentQueue = paymentQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var (orderId, isPaid) = await _paymentQueue.DequeuePaymentAsync(stoppingToken);

                // Process the payment using the scoped DbContext
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var order = await dbContext.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.Status = isPaid ? OrderStatus.Paid : OrderStatus.Cancelled;
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}

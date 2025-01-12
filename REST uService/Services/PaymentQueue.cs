using System.Collections.Concurrent;
using System.Threading.Channels;

namespace REST_uService.Services
{
    public class PaymentQueue
    {
        private readonly Channel<(int OrderId, bool IsPaid)> _paymentChannel;

        public PaymentQueue()
        {
            // Set up the channel with an unbounded capacity
            _paymentChannel = Channel.CreateUnbounded<(int, bool)>();
        }

        public async Task EnqueuePaymentAsync(int orderId, bool isPaid)
        {
            await _paymentChannel.Writer.WriteAsync((orderId, isPaid));
        }

        public async Task<(int OrderId, bool IsPaid)> DequeuePaymentAsync(CancellationToken cancellationToken)
        {
            return await _paymentChannel.Reader.ReadAsync(cancellationToken);
        }
    }
}

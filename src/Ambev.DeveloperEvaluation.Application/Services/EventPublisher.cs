using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Infrastructure.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event);
    }

    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;
        public EventPublisher(ILogger<EventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T @event)
        {
            string eventData = JsonSerializer.Serialize(@event);
            _logger.LogInformation("Event published: {EventType} - Data: {EventData}", typeof(T).Name, eventData);
            // Here you can add additional logic to forward the event (e.g., to a message bus)
            return Task.CompletedTask;
        }
    }
}
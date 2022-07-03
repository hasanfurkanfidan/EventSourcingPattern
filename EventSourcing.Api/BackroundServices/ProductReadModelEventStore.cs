using EventSourcing.Api.EventStores;
using EventSourcing.Api.Models;
using EventSourcing.Shared.Events;
using EventStore.ClientAPI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing.Api.BackroundServices
{
    public class ProductReadModelEventStore : BackgroundService
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProductReadModelEventStore> _logger;
        public ProductReadModelEventStore(IEventStoreConnection eventStoreConnection, IServiceProvider serviceProvider, ILogger<ProductReadModelEventStore> logger)
        {
            _eventStoreConnection = eventStoreConnection;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventStoreConnection.ConnectToPersistentSubscriptionAsync(ProductStream.StreamName, ProductStream.GroupName, EventAppered, autoAck: false);
            throw new System.NotImplementedException();
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        private async Task EventAppered(EventStorePersistentSubscriptionBase arg1, ResolvedEvent arg2)
        {
            _logger.LogInformation("Message Processing");
            var type = Type.GetType($"{Encoding.UTF8.GetString(arg2.Event.Metadata)}, EventSourcing.Shared");
            var eventData = Encoding.UTF8.GetString(arg2.Event.Data);

            var @event = JsonSerializer.Deserialize(eventData, type);

            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Product product = null;

            switch (@event)
            {
                case ProductCreatedEvent productCreatedEvent:
                    product = new Product
                    {
                        Id = productCreatedEvent.Id,
                        Name = productCreatedEvent.Name,
                        Price = productCreatedEvent.Price,
                        Stock = productCreatedEvent.Stock,
                        UserId = productCreatedEvent.UserId
                    };
                    context.Products.Add(product);
                    break;

                case ProductNameChangedEvent productNameChangedEvent:
                    product = context.Products.Find(productNameChangedEvent.Id);
                    if (product != null)
                        product.Name = productNameChangedEvent.ChangedName;
                    break;

                case ProductPriceChangedEvent productPriceChangedEvent:
                    product = context.Products.Find(productPriceChangedEvent.Id);
                    if (product != null)

                        product.Price = productPriceChangedEvent.ChangedPrice;
                    break;

                case ProductDeletedEvent productDeletedEvent:
                    product = context.Products.Find(productDeletedEvent.Id);
                    if (product != null)
                        context.Remove(product);
                    break;

            }

            context.SaveChanges();

            arg1.Acknowledge(arg2.Event.EventId);
        }
    }
}

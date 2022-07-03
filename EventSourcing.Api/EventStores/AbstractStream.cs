using EventSourcing.Shared.Events;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSourcing.Api.EventStores
{
    public abstract class AbstractStream
    {
        protected readonly LinkedList<IEvent> Events= new LinkedList<IEvent>();
        public string StreamName { get; set; }
        private readonly IEventStoreConnection _eventStoreConnection;
        public AbstractStream(string streamName,IEventStoreConnection eventStoreConnection)
        {
            StreamName = streamName;
            _eventStoreConnection = eventStoreConnection;
        }

        public async Task SaveAsync()
        {
            var newEvents = Events.ToList().Select(x => new EventData(
              Guid.NewGuid(),
              x.GetType().Name,
              true,
              Encoding.UTF8.GetBytes(JsonSerializer.Serialize(x, inputType: x.GetType())),
              Encoding.UTF8.GetBytes(x.GetType().FullName))).ToList();

            await _eventStoreConnection.AppendToStreamAsync(StreamName, ExpectedVersion.Any, newEvents);

            Events.Clear();
        }
    }
}

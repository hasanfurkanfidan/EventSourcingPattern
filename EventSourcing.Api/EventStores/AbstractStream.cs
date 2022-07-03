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
            var newEvents = Events.ToList().Select(e => new EventData(Guid.NewGuid()
                , e.GetType().Name
                , true
                , Encoding.UTF8.GetBytes(JsonSerializer.Serialize(e)),
                Encoding.UTF8.GetBytes(e.GetType().FullName)
                )).ToList();

            await _eventStoreConnection.AppendToStreamAsync(StreamName, ExpectedVersion.Any, newEvents);

            Events.Clear();
        }
    }
}

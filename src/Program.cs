using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DomainEventStore.Events;
using DomainEventStore.Model;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace DomainEventStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                IEventStoreConnection connection =
                    EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 2113));
                connection.ConnectAsync();

                var aggregateId = Guid.NewGuid();
                List<IEvent> eventsToRun = new List<IEvent>();

                // Commands
                // Domain Logic/Model
                // Events

                eventsToRun.Add(new AccountCreatedEvent(aggregateId, "Biyi Akinpelu"));
                eventsToRun.Add(new FundsDepositedEvent(aggregateId, 100));
                eventsToRun.Add(new FundsDepositedEvent(aggregateId, 20));
                eventsToRun.Add(new FundsWithdrawnEvent(aggregateId, 50));
                eventsToRun.Add(new FundsWithdrawnEvent(aggregateId, 4));
                eventsToRun.Add(new FundsDepositedEvent(aggregateId, 10));
                eventsToRun.Add(new FundsDepositedEvent(aggregateId, 20));

                foreach (var item in eventsToRun)
                {
                    var jsonString = JsonConvert.SerializeObject(item,
                        new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.None});
                    var jsonPayload = Encoding.UTF8.GetBytes(jsonString);
                    var eventStoreDataType =
                        new EventData(Guid.NewGuid(), item.GetType().Name, true, jsonPayload, null);
                    connection.AppendToStreamAsync(StreamId(aggregateId), ExpectedVersion.Any, eventStoreDataType);
                }

                var results = Task.Run(() => connection.ReadStreamEventsForwardAsync(StreamId(aggregateId),
                    StreamPosition.Start, 999, false));

                Task.WaitAll();

                var resultsData = results.Result;
                var bankState = new BankAccount();

                foreach (var evnt in resultsData.Events)
                {
                    var esJsonData = Encoding.UTF8.GetString(evnt.Event.Data);
                    if (evnt.Event.EventType == "AccountCreatedEvent")
                    {
                        var objState = JsonConvert.DeserializeObject<AccountCreatedEvent>(esJsonData);
                        bankState.Apply(objState);
                    }
                    else if (evnt.Event.EventType == "FundsDepositedEvent")
                    {
                        var objState = JsonConvert.DeserializeObject<FundsDepositedEvent>(esJsonData);
                        bankState.Apply(objState);
                    }
                    else
                    {
                        var objState = JsonConvert.DeserializeObject<AccountCreatedEvent>(esJsonData);
                        bankState.Apply(objState);
                    }
                }
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        private static string StreamId(Guid aggregateId)
        {
            return aggregateId.ToString();
        }
    }  
}

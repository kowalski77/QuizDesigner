using System;
using System.Reflection;
using System.Text.Json;
using QuizCreatedEvents;

namespace QuizDesigner.Application.Services.Outbox
{
    public static class OutboxSerializer
    {
        public static OutboxMessage Serialize(IIntegrationEvent message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var type = message.GetType().FullName ??
                       throw new InvalidOperationException("The type of the message cannot be null.");

            var data = JsonSerializer.Serialize(message);
            var outboxMessage = new OutboxMessage(Guid.NewGuid(), DateTime.UtcNow, type, data);

            return outboxMessage;
        }

        public static IIntegrationEvent Deserialize(OutboxMessage outboxMessage)
        {
            if (outboxMessage == null) throw new ArgumentNullException(nameof(outboxMessage));

            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetType(outboxMessage.Type) ?? 
                       throw new InvalidOperationException($"Could not find type {outboxMessage.Type}");

            var result = JsonSerializer.Deserialize(outboxMessage.Data, type) as IIntegrationEvent;

            return result ?? 
                   throw new InvalidOperationException($"Could not deserialize: {outboxMessage.Type}");
        }
    }
}
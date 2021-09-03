using System;
using System.Text.Json;

namespace QuizDesigner.Outbox
{
    public static class OutboxSerializer
    {
        public static OutboxMessage Serialize<T>(T message)
            where T : class
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var type = message.GetType();
            var data = JsonSerializer.Serialize(message, type);

            return new OutboxMessage(Guid.NewGuid(), DateTime.UtcNow, type.FullName ?? type.Name, data);
        }

        public static T Deserialize<T>(OutboxMessage outboxMessage)
            where T : class
        {
            if (outboxMessage == null) throw new ArgumentNullException(nameof(outboxMessage));

            var assembly = typeof(T).Assembly;
            var type = assembly.GetType(outboxMessage.Type) ?? 
                       throw new InvalidOperationException($"Could not find type {outboxMessage.Type}");

            var result = JsonSerializer.Deserialize(outboxMessage.Data, type) as T;

            return result ?? 
                   throw new InvalidOperationException($"Could not deserialize: {outboxMessage.Type}");
        }
    }
}
using System;

namespace QuizDesigner.Application.Services.Outbox
{
    public sealed class OutboxMessage
    {
        private OutboxMessage() { }

        public OutboxMessage(
            Guid id,
            DateTime date,
            string type,
            string data)
        {
            this.Id = id;
            this.OccurredOn = date;
            this.Type = type;
            this.Data = data;
            this.EventState = EventState.Unknown;
        }

        public Guid Id { get; private set; }

        public DateTime OccurredOn { get; private set; }

        public string Type { get; private set; } = string.Empty;

        public string Data { get; private set; } = string.Empty;

        public EventState EventState { get; private set; }

        public void Set(EventState eventState)
        {
            this.EventState = eventState;
        }
    }
}
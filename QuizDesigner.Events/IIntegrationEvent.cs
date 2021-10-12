using System;

namespace QuizDesigner.Events
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }
    }
}
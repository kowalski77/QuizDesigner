using System;

// ReSharper disable once CheckNamespace
namespace QuizCreatedEvents
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }
    }
}
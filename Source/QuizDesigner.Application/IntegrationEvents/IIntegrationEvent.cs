using System;

namespace QuizDesigner.Application.IntegrationEvents
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }
    }
}
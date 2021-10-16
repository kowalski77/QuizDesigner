using System;
using System.Collections.Generic;
using MediatR;

namespace QuizDesigner.Application.Messaging.IntegrationEventHandlers
{
    public sealed record ExamFinishedNotification(
        Guid Id,
        Guid QuizId,
        bool Passed,
        string Candidate,
        IEnumerable<string> CorrectQuestionsCollection,
        IEnumerable<string> WrongQuestionsCollection) : INotification;
}
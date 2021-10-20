using System;
using System.Collections.Generic;
using MediatR;

namespace QuizDesigner.Application.IntegrationEvents
{
    public sealed record ExamFinishedNotification(
        Guid Id,
        Guid QuizId,
        bool Passed,
        string Candidate,
        IEnumerable<string> CorrectQuestionsCollection,
        IEnumerable<string> WrongQuestionsCollection) : INotification;
}
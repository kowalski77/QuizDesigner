using System;
using MediatR;
using QuizDesigner.AzureServiceBus;
using QuizDesigner.Events;

namespace QuizDesigner.Application.IntegrationEvents
{
    public sealed class ExamFinishedTranslator : ITranslator<ExamFinished>
    {
        public INotification Translate(ExamFinished message)
        {
            var (id, summary) = message ?? throw new ArgumentNullException(nameof(message));

            var notification = new ExamFinishedNotification(
                id,
                summary.QuizId,
                summary.Passed,
                summary.Candidate,
                summary.CorrectQuestionsCollection,
                summary.WrongQuestionsCollection);

            return notification;
        }
    }
}
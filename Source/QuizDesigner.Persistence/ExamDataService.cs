using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application;
using QuizDesigner.Application.Messaging.IntegrationEventHandlers;

namespace QuizDesigner.Persistence
{
    public sealed class ExamDataService : IExamDataService
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public ExamDataService(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task AddAsync(ExamFinishedNotification examFinished, CancellationToken cancellationToken = default)
        {
            if (examFinished == null)
            {
                throw new ArgumentNullException(nameof(examFinished));
            }

            await using var context = this.contextFactory.CreateDbContext();

            var exam = new Exam(
                examFinished.Id, 
                new Summary(
                    examFinished.QuizId, examFinished.Passed, examFinished.Candidate, 
                    examFinished.CorrectQuestionsCollection, examFinished.WrongQuestionsCollection));

            context.Add(exam);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }
    }
}
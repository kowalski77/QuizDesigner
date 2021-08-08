using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizDesigner.Persistence.Support;
using QuizDesigner.Services;
using QuizDesigner.Services.Domain;

namespace QuizDesigner.Persistence
{
    public sealed class QuestionsRepository : IQuestionsRepository
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;
        private readonly ILogger<QuestionsRepository> logger;

        public QuestionsRepository(IDbContextFactory<QuizDesignerContext> contextFactory, ILogger<QuestionsRepository> logger)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> AddRangeAsync(IEnumerable<Question> questions, CancellationToken cancellationToken = default)
        {
            await using var dbOperation = DbOperation.With(this.contextFactory.CreateDbContext());

            var result = await dbOperation
                .Handle<DbUpdateException>()
                .AddLogging(this.logger)
                .ExecuteAsync(async (context) => await context.AddRangeAsync(questions, cancellationToken).ConfigureAwait(true))
                .ConfigureAwait(true);

            return result;
        }

        public async Task<Result> AddAnswersAsync(Guid questionId, IEnumerable<Answer> answerCollection, CancellationToken cancellationToken = default)
        {
            await using var dbOperation = DbOperation.With(this.contextFactory.CreateDbContext());

            var result = await dbOperation
                .Handle<DbUpdateException>()
                .AddLogging(this.logger)
                .ExecuteAsync(async (context) =>
                {
                    var question = await context.FindAsync<Question>(new object[] { questionId }, cancellationToken).ConfigureAwait(true);
                    if (question is null)
                    {
                        return Result.Fail(nameof(questionId), $"Question with id: {questionId} not found");
                    }

                    await context.Entry(question).Collection(x => x.Answers).LoadAsync(cancellationToken).ConfigureAwait(true);
                    foreach (var answer in question.Answers)
                    {
                        context.Remove(answer);
                    }

                    question.AddAnswers(answerCollection);

                    return Result.Ok();
                })
                .ConfigureAwait(true);

            return result;
        }
    }
}
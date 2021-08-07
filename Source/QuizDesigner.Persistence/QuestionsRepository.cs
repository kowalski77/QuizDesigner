using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizDesigner.Persistence.Support;
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
            var result = await Attempt
                .Handle<DbUpdateException>(this.logger, nameof(this.AddRangeAsync))
                .Try(async () =>
                {
                    await using var context = this.contextFactory.CreateDbContext();

                    await context.AddRangeAsync(questions, cancellationToken).ConfigureAwait(true);
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
                })
                .ConfigureAwait(true);

            return result;
        }

        public async Task<Result> RemoveAnswersAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            var result = await Attempt
                .Handle<DbUpdateException>(this.logger, nameof(this.AddRangeAsync))
                .Try(async () =>
                {
                    await using var context = this.contextFactory.CreateDbContext();

                    var question = await context.FindAsync<Question>(new object[] { questionId }, cancellationToken).ConfigureAwait(true);
                    await context.Entry(question).Collection(x => x.Answers).LoadAsync(cancellationToken).ConfigureAwait(true);

                    foreach (var answer in question.Answers)
                    {
                        context.Remove(answer);
                    }

                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
                })
                .ConfigureAwait(true);

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application;
using QuizDesigner.Persistence.Support;

namespace QuizDesigner.Persistence
{
    public sealed class QuestionsDataService : IQuestionsDataService
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public QuestionsDataService(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<Question> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();
            context.ActiveReadOnlyMode();

            var question = await context.Questions!
                .FirstAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(true);

            return question;
        }

        public async Task AddAsync(Question question, CancellationToken cancellationToken = default)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            await using var context = this.contextFactory.CreateDbContext();
            context.Add(question);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task AddAnswersAsync(Guid questionId, IEnumerable<Answer> answerCollection, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            var question = await context.Questions!.FirstAsync(x => x.Id == questionId, cancellationToken).ConfigureAwait(true);

            await context.Entry(question).Collection(x => x.Answers).LoadAsync(cancellationToken).ConfigureAwait(true);
            foreach (var answer in question.Answers)
            {
                context.Remove(answer);
            }

            question.AddAnswers(answerCollection);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task UpdateAsync(Question question, CancellationToken cancellationToken = default)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            await using var context = this.contextFactory.CreateDbContext();
            context.Attach(question);
            context.Entry(question).Property(x => x.Text).IsModified = true;
            context.Entry(question).Property(x => x.Tag).IsModified = true;
            context.Entry(question).Property(x => x.Difficulty).IsModified = true;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task RemoveAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();
            var question = await context.Questions!.FirstAsync(x => x.Id == questionId, cancellationToken).ConfigureAwait(true);

            question.SoftDeleted = true;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }
    }
}
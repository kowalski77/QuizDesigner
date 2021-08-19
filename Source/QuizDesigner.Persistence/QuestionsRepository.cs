using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Services;

namespace QuizDesigner.Persistence
{
    public sealed class QuestionsRepository : IQuestionsRepository
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public QuestionsRepository(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<Result> AddAsync(Question question, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            context.Add(question);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return Result.Ok();
        }

        public async Task<Result> AddRangeAsync(IEnumerable<Question> questions, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            await context.AddRangeAsync(questions, cancellationToken).ConfigureAwait(true);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
            
            return Result.Ok();
        }

        public async Task<Result> AddAnswersAsync(Guid questionId, IEnumerable<Answer> answerCollection, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

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
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(QuestionUpdatedDto questionUpdated, CancellationToken cancellationToken = default)
        {
            if (questionUpdated == null) throw new ArgumentNullException(nameof(questionUpdated));

            await using var context = this.contextFactory.CreateDbContext();
            var question = await context.FindAsync<Question>(new object[] { questionUpdated.Id }, cancellationToken).ConfigureAwait(true);
            if (question is null)
            {
                return Result.Fail(nameof(questionUpdated), $"Question with id: {questionUpdated.Id} not found");
            }

            question.SetText(questionUpdated.Text);
            question.SetTag(questionUpdated.Tag);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return Result.Ok();
        }

        public async Task<Result> RemoveAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();
            var question = await context.FindAsync<Question>(new object[] { questionId }, cancellationToken).ConfigureAwait(true);

            if (question is null)
            {
                return Result.Fail(nameof(questionId), $"Question with id: {questionId} not found");
            }

            question.Remove();
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return Result.Ok();
        }
    }
}
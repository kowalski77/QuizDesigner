using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application;

namespace QuizDesigner.Persistence
{
    public sealed class QuestionsDataService : IQuestionsDataService
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public QuestionsDataService(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task AddAsync(CreateQuestionDto question, CancellationToken cancellationToken = default)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            await using var context = this.contextFactory.CreateDbContext();

            var newlyQuestion = new Question(question.Text, question.Tag);
            context.Add(newlyQuestion);

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

        public async Task UpdateAsync(UpdateQuestionDto questionUpdated, CancellationToken cancellationToken = default)
        {
            if (questionUpdated == null)
            {
                throw new ArgumentNullException(nameof(questionUpdated));
            }

            await using var context = this.contextFactory.CreateDbContext();
            var question = await context.Questions!.FirstAsync(x => x.Id == questionUpdated.Id, cancellationToken).ConfigureAwait(true);

            question.SetText(questionUpdated.Text);
            question.SetTag(questionUpdated.Tag);

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
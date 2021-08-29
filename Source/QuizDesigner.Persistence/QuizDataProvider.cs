using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application;
using QuizDesigner.Persistence.Support;

namespace QuizDesigner.Persistence
{
    public class QuizDataProvider : IQuizDataProvider
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public QuizDataProvider(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<Quiz> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();
            context.ActiveReadOnlyMode();

            var quiz = await context.Quizzes!
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(true);

            return quiz;
        }

        public async Task<Quiz> GetQuizWithQuestionsAndAnswersAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            var quiz = await context.Quizzes!
                .Include(x => x.QuizQuestionCollection)
                .ThenInclude(x => x.Question)
                .ThenInclude(x => x!.Answers)
                .FirstAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(true);

            return quiz;
        }
    }
}

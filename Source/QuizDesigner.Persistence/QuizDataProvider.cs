using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application;
using QuizDesigner.Application.Queries;
using QuizDesigner.Application.Queries.Quizzes;
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
                .AsSingleQuery()
                .FirstAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(true);

            return quiz;
        }

        public async Task<Quiz> GetQuizAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            var quiz = await context.Quizzes!
                .Include(x=>x.QuizQuestionCollection)
                .ThenInclude(x=>x.Question)
                .AsSingleQuery()
                .FirstAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(true);

            return quiz;
        }

        public async Task<IPaginatedModel<QuizDto>> GetQuizzesAsync(QuizzesQuery query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            await using var context = this.contextFactory.CreateDbContext();
            context.ActiveReadOnlyMode();

            var quizzes = context.Quizzes!
                .Map()
                .SortQuizzesBy(query.SortByOptions, query.AscendingSort)
                .FilterQuizzesBy(query.FilterByOptions, query.FilterValue);

            var paginatedModel = new PaginatedModel<QuizDto>(quizzes, query.PageNumber, query.PageSize);
            await paginatedModel.PageAsync(cancellationToken).ConfigureAwait(true);

            return paginatedModel;
        }
    }
}
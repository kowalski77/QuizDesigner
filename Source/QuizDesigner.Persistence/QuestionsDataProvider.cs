using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application;
using QuizDesigner.Application.Queries;
using QuizDesigner.Persistence.Support;

namespace QuizDesigner.Persistence
{
    public sealed class QuestionsDataProvider : IQuestionsDataProvider
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public QuestionsDataProvider(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<IPaginatedModel<QuestionDto>> GetQuestionsAsync(QuestionsQuery questionsQuery, CancellationToken cancellationToken = default)
        {
            if (questionsQuery == null)
            {
                throw new ArgumentNullException(nameof(questionsQuery));
            }

            await using var context = this.contextFactory.CreateDbContext();
            context.ActiveReadOnlyMode();

            var questions = context.Questions!
                .Map()
                .SortQuestionsBy(questionsQuery.SortByOptions, questionsQuery.AscendingSort)
                .FilterQuestionsBy(questionsQuery.FilterByOptions, questionsQuery.FilterValue);

            var paginatedModel = new PaginatedModel<QuestionDto>(questions, questionsQuery.PageNumber, questionsQuery.PageSize);
            await paginatedModel.PageAsync(cancellationToken).ConfigureAwait(true);

            return paginatedModel;
        }

        public async Task<Maybe<QuestionDto>> GetQuestionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();
            context.ActiveReadOnlyMode();

            var question = await context.Questions!.Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(false);

            if (question == null)
            {
                return new Maybe<QuestionDto>();
            }

            var questionDto = new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                Tag = question.Tag,
                AnswerCollection = question.Answers.Select(x => new AnswerDto
                {
                    Text = x.Text,
                    IsCorrect = x.IsCorrect
                })
            };

            return questionDto;
        }

        public async Task<IReadOnlyList<string>> GetTags(CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();
            context.ActiveReadOnlyMode();

            var tags = await context.Questions!.Select(x => x.Tag)
                .Distinct()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(true);

            return tags;
        }

        public async Task<IReadOnlyList<KeyValuePair<Guid, string>>> GetQuestionsAsync(string tag, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();
            context.ActiveReadOnlyMode();

            var questions = await context.Questions!
                .Where(x => x.Tag == tag && x.Answers.Any())
                .Select(x => new KeyValuePair<Guid, string>(x.Id, x.Text))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(true);

            return questions;
        }
    }
}
﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Services;
using QuizDesigner.Services.Queries;

namespace QuizDesigner.Persistence
{
    public class QuestionsProvider : IQuestionsProvider
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public QuestionsProvider(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<IPaginatedModel<QuestionDto>> GetQuestionsAsync(QuestionsQuery questionsQuery, CancellationToken cancellationToken = default)
        {
            if (questionsQuery == null) throw new ArgumentNullException(nameof(questionsQuery));

            await using var context = this.contextFactory.CreateDbContext();
            SetChangeTrackerOptions(context);

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
            SetChangeTrackerOptions(context);

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
                AnswerCollection = question.Answers.Select(x=> new AnswerDto
                {
                    Text = x.Text,
                    IsCorrect = x.IsCorrect
                })
            };

            return questionDto;
        }

        private static void SetChangeTrackerOptions(DbContext context)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
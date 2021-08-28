using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Persistence.Support;
using QuizDesigner.Services;

namespace QuizDesigner.Persistence
{
    public sealed class DesignerService : IDesignerService
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public DesignerService(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
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

        public async Task<Result<Guid>> CreateQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default)
        {
            if (createQuizDto == null) throw new ArgumentNullException(nameof(createQuizDto));

            await using var context = this.contextFactory.CreateDbContext();

            var quiz = new Quiz(createQuizDto.Name, createQuizDto.ExamName);
            quiz.AddQuestions(createQuizDto.QuestionIdCollection);

            var quizEntry = context.Add(quiz);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return Result.Ok(quizEntry.Entity.Id);
        }

        public async Task<Result> UpdateQuizAsync(UpdateQuizDto updateQuizDto, CancellationToken cancellationToken = default)
        {
            if (updateQuizDto == null) throw new ArgumentNullException(nameof(updateQuizDto));

            await using var context = this.contextFactory.CreateDbContext();

            var quiz = await context.FindAsync<Quiz>(new object[] { updateQuizDto.QuizId }, cancellationToken).ConfigureAwait(true);
            if (quiz is null)
            {
                return Result.Fail<Guid>(nameof(updateQuizDto.QuizId), $"Quiz with id: {updateQuizDto.QuizId} not found");
            }

            await context.Entry(quiz).Collection(x => x.QuizQuestionCollection).LoadAsync(cancellationToken).ConfigureAwait(true);

            quiz.Update(updateQuizDto.Name, updateQuizDto.ExamName);
            quiz.UpdateQuestions(updateQuizDto.QuestionIdCollection);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return Result.Ok();
        }
    }
}
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

        public async Task<Result<Guid>> CreateDraftQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default)
        {
            if (createQuizDto == null) throw new ArgumentNullException(nameof(createQuizDto));

            await using var context = this.contextFactory.CreateDbContext();

            var quiz = new Quiz
            {
                Name = createQuizDto.Name,
                ExamName = createQuizDto.ExamName,
                Draft = true
            };

            foreach (var id in createQuizDto.QuestionIdCollection)
            {
                var question = await context.Questions!.FirstAsync(x => x.Id == id, cancellationToken).ConfigureAwait(true);
                quiz.Questions.Add(question);
            }

            var quizEntry = context.Add(quiz);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return Result.Ok(quizEntry.Entity.Id);
        }

        public async Task<Result> SaveQuizAsync(Guid quizId, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            var quiz = await context.FindAsync<Quiz>(new object[] { quizId }, cancellationToken).ConfigureAwait(true);
            if (quiz == null)
            {
                return Result.Fail(nameof(quizId), $"Quiz with id: {quizId} not found in database");
            }

            quiz.Draft = false;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return Result.Ok();
        }
    }
}
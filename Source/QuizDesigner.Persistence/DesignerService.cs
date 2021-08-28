using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using QuizCreatedEvents;
using QuizDesigner.Persistence.Support;
using QuizDesigner.Services;

namespace QuizDesigner.Persistence
{
    public sealed class DesignerService : IDesignerService
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;
        private readonly IPublishEndpoint publishEndpoint;

        public DesignerService(
            IDbContextFactory<QuizDesignerContext> contextFactory,
            IPublishEndpoint publishEndpoint)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
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

        public async Task<Guid> CreateQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default)
        {
            if (createQuizDto == null) throw new ArgumentNullException(nameof(createQuizDto));

            await using var context = this.contextFactory.CreateDbContext();

            var quiz = new Quiz(createQuizDto.Name, createQuizDto.ExamName);
            quiz.AddQuestions(createQuizDto.QuestionIdCollection);

            var quizEntry = context.Add(quiz);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return quizEntry.Entity.Id;
        }

        public async Task UpdateQuizAsync(UpdateQuizDto updateQuizDto, CancellationToken cancellationToken = default)
        {
            if (updateQuizDto == null) throw new ArgumentNullException(nameof(updateQuizDto));

            await using var context = this.contextFactory.CreateDbContext();

            var quiz = await context.FindAsync<Quiz>(new object[] { updateQuizDto.QuizId }, cancellationToken).ConfigureAwait(true);

            await context.Entry(quiz).Collection(x => x.QuizQuestionCollection).LoadAsync(cancellationToken).ConfigureAwait(true);

            quiz.Update(updateQuizDto.Name, updateQuizDto.ExamName);
            quiz.UpdateQuestions(updateQuizDto.QuestionIdCollection);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task PublishQuizAsync(Guid quizId, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            var quiz = await context.Quizzes!
                .Include(x => x.QuizQuestionCollection)
                .ThenInclude(x => x.Question)
                .ThenInclude(x => x!.Answers)
                .FirstAsync(x => x.Id == quizId, cancellationToken)
                .ConfigureAwait(true);

            await this.PublishQuizCreatedIntegrationEventAsync(quiz, cancellationToken).ConfigureAwait(true);

            quiz.SetAsPublished();

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        private async Task PublishQuizCreatedIntegrationEventAsync(Quiz quiz, CancellationToken cancellationToken)
        {
            var questions = quiz.QuizQuestionCollection.Select(x =>
                new ExamQuestion(x.Question!.Text, x.Question.Answers.Select(y =>
                    new ExamAnswer(y.Text, y.IsCorrect))));

            var quizCreated = new QuizCreated(quiz.Name, quiz.ExamName, questions);

            await this.publishEndpoint.Publish(quizCreated, cancellationToken).ConfigureAwait(true);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using QuizCreatedEvents;
using QuizDesigner.Application;

namespace QuizDesigner.Persistence
{
    public sealed class QuizDataService : IQuizDataService
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;
        private readonly IPublishEndpoint publishEndpoint;

        public QuizDataService(
            IDbContextFactory<QuizDesignerContext> contextFactory,
            IPublishEndpoint publishEndpoint)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task<Quiz> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            var quiz = await context.Quizzes!
                .Include(x => x.QuizQuestionCollection)
                .ThenInclude(x => x.Question)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(true);

            return quiz;
        }

        public async Task<Guid> CreateAsync(Quiz quiz, CancellationToken cancellationToken = default)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            await using var context = this.contextFactory.CreateDbContext();

            var quizEntry = context.Add(quiz);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return quizEntry.Entity.Id;
        }

        public async Task UpdateAsync(Quiz quiz, CancellationToken cancellationToken = default)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            await using var context = this.contextFactory.CreateDbContext();

            context.Attach(quiz).State = EntityState.Modified;

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
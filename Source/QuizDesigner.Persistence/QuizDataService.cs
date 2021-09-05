﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application;

namespace QuizDesigner.Persistence
{
    public sealed class QuizDataService : IQuizDataService
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public QuizDataService(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<Result<Guid>> CreateAsync(Quiz quiz, CancellationToken cancellationToken = default)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            try
            {
                await using var context = this.contextFactory.CreateDbContext();

                var quizEntry = context.Add(quiz);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

                return Result.Ok(quizEntry.Entity.Id);
            }
            catch(DbUpdateException e)
            {
                if(e.InnerException is not null && 
                    e.InnerException.Message.Contains("Cannot insert duplicate key row", StringComparison.InvariantCulture))
                {
                    return Result.Fail<Guid>(nameof(quiz.Name), $"Name: {quiz.Name} already exist in the database");
                }
                throw;
            }
        }

        public async Task Update(Quiz quiz, CancellationToken cancellationToken = default)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            await using var context = this.contextFactory.CreateDbContext();

            context.Attach(quiz);
            context.Entry(quiz).Property(x => x.IsPublished).IsModified = true;
            context.Entry(quiz).Property(x => x.Name).IsModified = true;
            context.Entry(quiz).Property(x => x.ExamName).IsModified = true;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task UpdateQuestionsAsync(Quiz quiz, CancellationToken cancellationToken = default)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            await using var context = this.contextFactory.CreateDbContext();

            var currentQuiz = await context.Quizzes!
                .Include(x => x.QuizQuestionCollection)
                .FirstAsync(x => x.Id == quiz.Id, cancellationToken)
                .ConfigureAwait(true);

            foreach (var quizQuestion in currentQuiz.QuizQuestionCollection)
            {
                context.Remove(quizQuestion);
            }

            currentQuiz.AddQuestions(quiz.QuizQuestionCollection.Select(x=>x.QuestionId));

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application;

namespace QuizDesigner.Persistence
{
    public sealed class ExamDataService : IExamDataService
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public ExamDataService(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<Exam> AddAsync(Exam exam, CancellationToken cancellationToken = default)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }

            await using var context = this.contextFactory.CreateDbContext();

            context.Add(exam);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return exam;
        }
    }
}
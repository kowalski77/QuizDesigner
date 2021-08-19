using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

            var tags = await context.Questions!.Select(x => x.Tag)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(true);

            return tags;
        }

        public async Task<IReadOnlyList<KeyValuePair<Guid, string>>> GetQuestionsAsync(CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            var questions = await context.Questions!.Select(x => new KeyValuePair<Guid, string>(x.Id, x.Text))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(true);

            return questions;
        }
    }
}
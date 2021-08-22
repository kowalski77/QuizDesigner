using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                .Where(x=>x.Tag == tag)
                .Select(x => new KeyValuePair<Guid, string>(x.Id, x.Text))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(true);

            return questions;
        }
    }
}
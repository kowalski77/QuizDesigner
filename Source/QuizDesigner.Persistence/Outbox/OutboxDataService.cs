using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application.Services.Outbox;
using QuizDesigner.Persistence.Support;

namespace QuizDesigner.Persistence.Outbox
{
    public sealed class OutboxDataService : IOutboxDataService
    {
        private readonly IDbContextFactory<OutboxDbContext> contextFactory;

        public OutboxDataService(IDbContextFactory<OutboxDbContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task SaveMessageAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
        {
            if (outboxMessage == null) throw new ArgumentNullException(nameof(outboxMessage));

            await using var context = this.contextFactory.CreateDbContext();

            context.Add(outboxMessage);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task UpdateMessageAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();

            context.Attach(outboxMessage);
            context.Entry(outboxMessage).Property(x => x.EventState).IsModified = true;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Maybe<IReadOnlyList<OutboxMessage>>> GetNotPublishedAsync(CancellationToken cancellationToken = default)
        {
            await using var context = this.contextFactory.CreateDbContext();
            context.ActiveReadOnlyMode();

            var outboxMessages = await context.OutboxMessages!
                .Where(e => e.EventState != EventState.Published)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(true);

            return outboxMessages;
        }
    }
}
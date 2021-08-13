using System;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace QuizDesigner.Persistence.Support
{
    public class DbOperation : IAsyncDisposable
    {
        private Func<Exception, Exception?>? exceptionPredicate;
        private ILogger? logger;

        private readonly DbContext context;

        private DbOperation(DbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public static DbOperation With(DbContext context)
        {
            return new DbOperation(context);
        }

        public DbOperation AddLogging(ILogger log)
        {
            this.logger = log ?? throw new ArgumentNullException(nameof(log));
            return this;
        }

        public DbOperation Handle<TException>()
            where TException : Exception
        {
            this.exceptionPredicate = exception => exception is TException ? exception : null;
            return this;
        }

        public async Task<Result> ExecuteAsync(Func<DbContext, Task> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            await action(this.context).ConfigureAwait(true);

            return await this.TrySaveChangesAsync().ConfigureAwait(true);
        }

        public async Task<Result> ExecuteAsync(Func<DbContext, Task<Result>> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var result = await action(this.context).ConfigureAwait(true);
            if (result.Failure)
            {
                return result;
            }

            return await this.TrySaveChangesAsync().ConfigureAwait(true);
        }

        public async ValueTask DisposeAsync()
        {
            await this.context.DisposeAsync().ConfigureAwait(true);
        }

        private async Task<Result> TrySaveChangesAsync()
        {
            try
            {
                var entries = await this.context.SaveChangesAsync().ConfigureAwait(true);

                this.logger?.LogDebug($"{nameof(this.context.SaveChangesAsync)} success with: {entries} entries written to database");

                return Result.Ok();
            }
            catch (Exception e)
            {
                var exception = this.exceptionPredicate?.Invoke(e);
                if (exception is null)
                {
                    throw;
                }
                this.logger?.LogWarning(e, e.Message);

                return Result.Fail(e.Source ?? string.Empty, e.Message);
            }
        }
    }
}
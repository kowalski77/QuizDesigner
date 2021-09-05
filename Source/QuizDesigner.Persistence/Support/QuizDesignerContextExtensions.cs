using System;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Microsoft.EntityFrameworkCore;

namespace QuizDesigner.Persistence.Support
{
    public static class QuizDesignerContextExtensions
    {
        public static async Task<Result> SaveAsync(this QuizDesignerContext context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

                return Result.Ok();
            }
            catch(DbUpdateException e)
            {
                if(e.InnerException is not null && 
                   e.InnerException.Message.Contains("Cannot insert duplicate key row", StringComparison.InvariantCulture))
                {
                    return Result.Fail(string.Empty, string.Empty);
                }
                throw;
            }
        }
    }
}
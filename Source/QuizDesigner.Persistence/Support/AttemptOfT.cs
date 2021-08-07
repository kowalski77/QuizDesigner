using System;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Microsoft.Extensions.Logging;

namespace QuizDesigner.Persistence.Support
{
    public class AttemptOfT<T> : Attempt
        where T : Exception
    {
        public AttemptOfT(ILogger logger, string actionName) 
            : base(logger, actionName)
        {
        }

        public async Task<Result> Try(Func<Task> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            try
            {
                await action().ConfigureAwait(true);
                return Result.Ok();
            }
            catch (T e)
            {
                this.Logger.LogError(e, e.Message);

                return Result.Fail(this.ActionName, e.Message);
            }
        }
    }
}
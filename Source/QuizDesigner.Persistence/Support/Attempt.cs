using System;
using Microsoft.Extensions.Logging;

namespace QuizDesigner.Persistence.Support
{
    public class Attempt
    {
        protected Attempt(ILogger logger, string actionName)
        {
            this.Logger = logger;
            this.ActionName = actionName;
        }

        protected ILogger Logger { get; }

        protected string ActionName { get;  }

        public static AttemptOfT<T> Handle<T>(ILogger logger, string actionName)
            where T : Exception
        {
            return new(logger, actionName);
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore;

namespace QuizDesigner.Persistence.Support
{
    public static class EfChangeTrackerExtensions
    {
        public static void ActiveReadOnlyMode(this DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
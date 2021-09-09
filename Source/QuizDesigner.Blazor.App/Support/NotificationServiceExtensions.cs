using System;
using System.Threading.Tasks;
using Blazorise;

namespace QuizDesigner.Blazor.App.Support
{
    public static class NotificationServiceExtensions
    {
        public static async Task ShowNoSelectedQuestionsError(this INotificationService notificationService)
        {
            if (notificationService == null)
            {
                throw new ArgumentNullException(nameof(notificationService));
            }

            await notificationService.Error("Please, drop questions to the right box to save the quiz", "Quiz empty").ConfigureAwait(true);
        }
    }
}
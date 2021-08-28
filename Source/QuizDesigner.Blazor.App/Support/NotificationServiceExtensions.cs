using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Blazorise;

namespace QuizDesigner.Blazor.App.Support
{
    public static class NotificationServiceExtensions
    {
        public static async Task ShowSaveQuestionFeedback(this INotificationService notificationService, Result result)
        {
            if (notificationService == null) throw new ArgumentNullException(nameof(notificationService));
            if (result == null) throw new ArgumentNullException(nameof(result));

            if (result.Success)
            {
                await notificationService.Success("Question successfully saved!").ConfigureAwait(true);
            }
            else
            {
                await notificationService.Error("An error occurred while saving the question", result.Error)
                    .ConfigureAwait(true);
            }
        }

        public static async Task ShowRemoveQuestionFeedback(this INotificationService notificationService, Result result)
        {
            if (notificationService == null) throw new ArgumentNullException(nameof(notificationService));
            if (result == null) throw new ArgumentNullException(nameof(result));

            if (result.Success)
            {
                await notificationService.Success("Question successfully removed!").ConfigureAwait(true);
            }
            else
            {
                await notificationService.Error("An error occurred while removing the question", result.Error)
                    .ConfigureAwait(true);
            }
        }

        public static async Task ShowNoSelectedQuestionsError(this INotificationService notificationService)
        {
            if (notificationService == null) throw new ArgumentNullException(nameof(notificationService));

            await notificationService.Error("Please, drop questions to the right box to save the quiz", "Quiz empty").ConfigureAwait(true);
        }
    }
}
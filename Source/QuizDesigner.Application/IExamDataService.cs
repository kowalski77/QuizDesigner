using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Application.Messaging.IntegrationEventHandlers;

namespace QuizDesigner.Application
{
    public interface IExamDataService
    {
        Task AddAsync(ExamFinishedNotification examFinished, CancellationToken cancellationToken = default);
    }
}
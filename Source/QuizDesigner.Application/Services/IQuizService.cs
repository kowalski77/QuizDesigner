using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application.Services
{
    public interface IQuizService
    {
        Task PublishQuizAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
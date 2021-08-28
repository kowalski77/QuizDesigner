using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuizDataService
    {
        Task<Guid> CreateQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default);

        Task UpdateQuizAsync(UpdateQuizDto updateQuizDto, CancellationToken cancellationToken = default);

        Task PublishQuizAsync(Guid quizId, CancellationToken cancellationToken = default);
    }
}
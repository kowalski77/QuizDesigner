using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuizDataService
    {
        Task<Guid> CreateAsync(Quiz quiz, CancellationToken cancellationToken = default);

        Task<Quiz> GetAsync(Guid id, CancellationToken cancellationToken = default);

        Task UpdateAsync(Quiz quiz, CancellationToken cancellationToken = default);

        Task PublishQuizAsync(Guid quizId, CancellationToken cancellationToken = default);
    }
}
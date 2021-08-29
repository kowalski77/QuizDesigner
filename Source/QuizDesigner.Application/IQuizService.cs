using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuizService
    {
        Task<Guid> CreateQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default);

        Task UpdateQuizAsync(UpdateQuizDto updateQuizDto, CancellationToken cancellationToken = default);

        Task PublishQuizAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuizDataService
    {
        Task<Guid> CreateAsync(Quiz quiz, CancellationToken cancellationToken = default);

        Task UpdateQuestionsAsync(Quiz quiz, CancellationToken cancellationToken = default);

        Task Update(Quiz quiz, CancellationToken cancellationToken = default);
    }
}
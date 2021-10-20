using Arch.Utils.Functional.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuizDataService
    {
        Task<Quiz> GetAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Quiz> GetQuizWithQuestionsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Quiz> GetQuizWithQuestionsAndAnswersAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Result<Guid>> CreateAsync(Quiz quiz, CancellationToken cancellationToken = default);

        Task<Result> Update(Quiz quiz, CancellationToken cancellationToken = default);

        Task UpdateQuestionsAsync(Quiz quiz, CancellationToken cancellationToken = default);

        Task RemoveAsync(Guid quizId, CancellationToken cancellationToken = default);
    }
}
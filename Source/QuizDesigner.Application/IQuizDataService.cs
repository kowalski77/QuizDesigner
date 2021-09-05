using Arch.Utils.Functional.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuizDataService
    {
        Task<Result<Guid>> CreateAsync(Quiz quiz, CancellationToken cancellationToken = default);

        Task<Result> Update(Quiz quiz, CancellationToken cancellationToken = default);

        Task UpdateQuestionsAsync(Quiz quiz, CancellationToken cancellationToken = default);
    }
}
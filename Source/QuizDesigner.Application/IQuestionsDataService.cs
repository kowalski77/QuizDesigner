using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuestionsDataService
    {
        Task<Question> GetAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddAsync(Question question, CancellationToken cancellationToken = default);

        Task AddAnswersAsync(Guid questionId, IEnumerable<Answer> answerCollection, CancellationToken cancellationToken = default);

        Task UpdateAsync(Question question, CancellationToken cancellationToken = default);

        Task RemoveAsync(Guid questionId, CancellationToken cancellationToken = default);
    }
}
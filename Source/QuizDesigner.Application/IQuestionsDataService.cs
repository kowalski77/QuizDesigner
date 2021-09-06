using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuestionsDataService
    {
        Task AddAsync(Question question, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<Question> questions, CancellationToken cancellationToken = default);

        Task AddAnswersAsync(Guid questionId, IEnumerable<Answer> answerCollection, CancellationToken cancellationToken = default);

        Task UpdateAsync(UpdateQuestionDto questionUpdated, CancellationToken cancellationToken = default);

        Task RemoveAsync(Guid questionId, CancellationToken cancellationToken = default);
    }
}
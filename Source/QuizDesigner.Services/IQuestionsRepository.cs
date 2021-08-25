using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;

namespace QuizDesigner.Services
{
    public interface IQuestionsRepository
    {
        Task<Result> AddAsync(Question question, CancellationToken cancellationToken = default);

        Task<Result> AddRangeAsync(IEnumerable<Question> questions, CancellationToken cancellationToken = default);

        Task<Result> AddAnswersAsync(Guid questionId, IEnumerable<Answer> answerCollection, CancellationToken cancellationToken = default);

        Task<Result> UpdateAsync(UpdateQuestionDto questionUpdated, CancellationToken cancellationToken = default);

        Task<Result> RemoveAsync(Guid questionId, CancellationToken cancellationToken = default);
    }
}
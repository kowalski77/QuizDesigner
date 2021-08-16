using System;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;
using QuizDesigner.Services.Queries;

namespace QuizDesigner.Services
{
    public interface IQuestionsProvider
    {
        Task<IPaginatedModel<QuestionDto>> GetQuestionsAsync(QuestionsQuery questionsQuery, CancellationToken cancellationToken = default);

        Task<Maybe<QuestionDto>> GetQuestionAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
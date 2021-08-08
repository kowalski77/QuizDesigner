using System;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;
using QuizDesigner.Services.Queries;

namespace QuizDesigner.Services
{
    public interface IQuestionsProvider
    {
        Task<PaginatedModel<QuestionDto>> GetQuestionsAsync(QuestionsQuery questionsQuery);

        Task<Maybe<QuestionDto>> GetQuestionAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
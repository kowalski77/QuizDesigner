using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;
using QuizDesigner.Application.Queries;

namespace QuizDesigner.Application
{
    public interface IQuestionsProvider
    {
        Task<IPaginatedModel<QuestionDto>> GetQuestionsAsync(QuestionsQuery questionsQuery, CancellationToken cancellationToken = default);

        Task<Maybe<QuestionDto>> GetQuestionAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<string>> GetTags(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<KeyValuePair<Guid, string>>> GetQuestionsAsync(string tag, CancellationToken cancellationToken = default);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Services
{
    public interface IDesignerService
    {
        Task<IReadOnlyList<string>> GetTags(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<KeyValuePair<Guid, string>>> GetQuestionsAsync(string tag, CancellationToken cancellationToken = default);

        Task<Guid> CreateQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default);

        Task UpdateQuizAsync(UpdateQuizDto updateQuizDto, CancellationToken cancellationToken = default);

        Task PublishQuizAsync(Guid quizId, CancellationToken cancellationToken = default);
    }
}
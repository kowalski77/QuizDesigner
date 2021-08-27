using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;

namespace QuizDesigner.Services
{
    public interface IDesignerService
    {
        Task<IReadOnlyList<string>> GetTags(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<KeyValuePair<Guid, string>>> GetQuestionsAsync(string tag, CancellationToken cancellationToken = default);

        Task<Result<Guid>> CreateQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default);

        Task<Result<Guid>> UpdateQuizAsync(UpdateQuizDto updateQuizDto, CancellationToken cancellationToken = default);
    }
}
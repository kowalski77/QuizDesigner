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
    }
}
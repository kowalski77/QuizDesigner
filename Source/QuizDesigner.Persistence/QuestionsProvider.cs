using System;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Services;
using QuizDesigner.Services.Queries;

namespace QuizDesigner.Persistence
{
    public class QuestionsProvider : IQuestionsProvider
    {
        private readonly IDbContextFactory<QuizDesignerContext> contextFactory;

        public QuestionsProvider(IDbContextFactory<QuizDesignerContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }


        public async Task<PaginatedModel<QuestionDto>> GetQuestionsAsync(QuestionsQuery questionsQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<Maybe<QuestionDto>> GetQuestionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
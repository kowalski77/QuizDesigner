using System;
using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Application.Queries;
using QuizDesigner.Application.Queries.Quizzes;

namespace QuizDesigner.Application
{
    public interface IQuizDataProvider
    {
        // TODO:  return dto
        Task<Quiz> GetQuizWithQuestionsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IPaginatedModel<QuizDto>> GetQuizzesAsync(QuizzesQuery query, CancellationToken cancellationToken = default);
    }
}
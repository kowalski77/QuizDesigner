using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Application.Queries;
using QuizDesigner.Application.Queries.Quizzes;

namespace QuizDesigner.Application
{
    public interface IQuizDataProvider
    {
        Task<IPaginatedModel<QuizDto>> GetQuizzesAsync(QuizzesQuery query, CancellationToken cancellationToken = default);
    }
}
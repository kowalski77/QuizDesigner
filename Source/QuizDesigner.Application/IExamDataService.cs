using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IExamDataService
    {
        Task<Exam> AddAsync(Exam exam, CancellationToken cancellationToken = default);
    }
}
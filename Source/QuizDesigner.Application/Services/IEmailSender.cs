using System.Threading.Tasks;

namespace QuizDesigner.Application.Services
{
    public interface IEmailSender
    {
        Task SendAsync(EmailContents emailContents);
    }
}
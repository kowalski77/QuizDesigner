using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Events;

namespace QuizDesigner.Application.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizDataService quizDataService;
        private readonly IChannelService channelService;

        public QuizService(
            IQuizDataService quizDataService,
            IChannelService channelService)
        {
            this.quizDataService = quizDataService ?? throw new ArgumentNullException(nameof(quizDataService));
            this.channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
        }

        public async Task PublishQuizAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var quiz = await this.quizDataService.GetQuizWithQuestionsAndAnswersAsync(id, cancellationToken).ConfigureAwait(true);

            quiz.SetAsPublished();
            await this.quizDataService.Update(quiz, cancellationToken).ConfigureAwait(true);

            await this.PublishQuizCreatedIntegrationEventAsync(quiz).ConfigureAwait(true);
        }

        private async Task PublishQuizCreatedIntegrationEventAsync(Quiz quiz)
        {
            var questions = quiz.QuizQuestionCollection.Select(x =>
                new Events.ExamQuestion(x.Question!.Text, x.Question!.Tag, (int)x.Question!.Difficulty, x.Question.Answers.Select(y =>
                    new ExamAnswer(y.Text, y.IsCorrect))));

            var quizCreated = new QuizCreated(Guid.NewGuid(), quiz.Id, quiz.Name, quiz.Category, questions);

            await this.channelService.AddAsync(quizCreated, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
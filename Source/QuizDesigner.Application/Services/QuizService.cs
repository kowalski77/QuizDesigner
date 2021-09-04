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
        private readonly IQuizDataProvider quizDataProvider;
        private readonly IChannelService channelService;

        public QuizService(
            IQuizDataService quizDataService,
            IQuizDataProvider quizDataProvider,
            IChannelService channelService)
        {
            this.quizDataService = quizDataService ?? throw new ArgumentNullException(nameof(quizDataService));
            this.quizDataProvider = quizDataProvider ?? throw new ArgumentNullException(nameof(quizDataProvider));
            this.channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
        }

        public async Task<Guid> CreateQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default)
        {
            if (createQuizDto == null) throw new ArgumentNullException(nameof(createQuizDto));

            var quiz = new Quiz(createQuizDto.Name, createQuizDto.ExamName);
            quiz.AddQuestions(createQuizDto.QuestionIdCollection);

            return await this.quizDataService.CreateAsync(quiz, cancellationToken).ConfigureAwait(true);
        }

        // TODO: Result with success or not, maybe UNIQUE constraint with Quiz Name.
        public async Task UpdateQuizAsync(UpdateQuizDto updateQuizDto, CancellationToken cancellationToken = default)
        {
            if (updateQuizDto == null) throw new ArgumentNullException(nameof(updateQuizDto));

            var quiz = await this.quizDataProvider.GetAsync(updateQuizDto.QuizId, cancellationToken).ConfigureAwait(true);

            quiz.SetNames(updateQuizDto.Name, updateQuizDto.ExamName);
            quiz.SetQuestions(updateQuizDto.QuestionIdCollection);

            await this.quizDataService.Update(quiz, cancellationToken).ConfigureAwait(true);
            await this.quizDataService.UpdateQuestionsAsync(quiz, cancellationToken).ConfigureAwait(true);
        }

        public async Task PublishQuizAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var quiz = await this.quizDataProvider.GetQuizWithQuestionsAndAnswersAsync(id, cancellationToken).ConfigureAwait(true);

            quiz.SetAsPublished();
            await this.quizDataService.Update(quiz, cancellationToken).ConfigureAwait(true);

            await this.PublishQuizCreatedIntegrationEventAsync(quiz).ConfigureAwait(true);
        }

        private async Task PublishQuizCreatedIntegrationEventAsync(Quiz quiz)
        {
            var questions = quiz.QuizQuestionCollection.Select(x =>
                new ExamQuestion(x.Question!.Text, x.Question.Answers.Select(y =>
                    new ExamAnswer(y.Text, y.IsCorrect))));

            var quizCreated = new QuizCreated(Guid.NewGuid(), quiz.Name, quiz.ExamName, questions);

            await this.channelService.AddAsync(quizCreated, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
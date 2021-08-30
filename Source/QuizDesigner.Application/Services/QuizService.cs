using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Application.IntegrationEvents;
using QuizDesigner.Application.Services.Outbox;

namespace QuizDesigner.Application.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizDataService quizDataService;
        private readonly IQuizDataProvider quizDataProvider;
        private readonly IOutboxService outboxService;

        public QuizService(
            IQuizDataService quizDataService, 
            IQuizDataProvider quizDataProvider, 
            IOutboxService outboxService)
        {
            this.quizDataService = quizDataService ?? throw new ArgumentNullException(nameof(quizDataService));
            this.quizDataProvider = quizDataProvider ?? throw new ArgumentNullException(nameof(quizDataProvider));
            this.outboxService = outboxService ?? throw new ArgumentNullException(nameof(outboxService));
        }

        public async Task<Guid> CreateQuizAsync(CreateQuizDto createQuizDto, CancellationToken cancellationToken = default)
        {
            if (createQuizDto == null) throw new ArgumentNullException(nameof(createQuizDto));

            var quiz = new Quiz(createQuizDto.Name, createQuizDto.ExamName);
            quiz.AddQuestions(createQuizDto.QuestionIdCollection);

            return await this.quizDataService.CreateAsync(quiz, cancellationToken).ConfigureAwait(true);
        }

        public async Task UpdateQuizAsync(UpdateQuizDto updateQuizDto, CancellationToken cancellationToken = default)
        {
            if (updateQuizDto == null) throw new ArgumentNullException(nameof(updateQuizDto));

            var quiz = await this.quizDataProvider.GetAsync(updateQuizDto.QuizId, cancellationToken).ConfigureAwait(true);

            quiz.Update(updateQuizDto.Name, updateQuizDto.ExamName);
            quiz.UpdateQuestions(updateQuizDto.QuestionIdCollection);

            await this.quizDataService.UpdateWithQuestionsAsync(quiz, cancellationToken).ConfigureAwait(true);
        }

        public async Task PublishQuizAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var quiz = await this.quizDataProvider.GetQuizWithQuestionsAndAnswersAsync(id, cancellationToken).ConfigureAwait(true);

            await this.PublishQuizCreatedIntegrationEventAsync(quiz, cancellationToken).ConfigureAwait(true);

            quiz.SetAsPublished();

            await this.quizDataService.Update(quiz, cancellationToken).ConfigureAwait(true);
        }

        private async Task PublishQuizCreatedIntegrationEventAsync(Quiz quiz, CancellationToken cancellationToken)
        {
            var questions = quiz.QuizQuestionCollection.Select(x =>
                new ExamQuestion(x.Question!.Text, x.Question.Answers.Select(y =>
                    new ExamAnswer(y.Text, y.IsCorrect))));

            var quizCreated = new QuizCreated(Guid.NewGuid(), quiz.Name, quiz.ExamName, questions);

            await this.outboxService.PublishIntegrationEventAsync(quizCreated, cancellationToken).ConfigureAwait(true);
        }
    }
}
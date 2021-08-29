using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public class QuizService : IQuizService
    {
        private readonly IQuizDataService quizDataService;

        public QuizService(IQuizDataService quizDataService)
        {
            this.quizDataService = quizDataService ?? throw new ArgumentNullException(nameof(quizDataService));
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

            var quiz = await this.quizDataService.GetAsync(updateQuizDto.QuizId, cancellationToken).ConfigureAwait(true);

            quiz.Update(updateQuizDto.Name, updateQuizDto.ExamName);
            quiz.UpdateQuestions(updateQuizDto.QuestionIdCollection);

            await this.quizDataService.UpdateAsync(quiz, cancellationToken).ConfigureAwait(true);
        }
    }
}
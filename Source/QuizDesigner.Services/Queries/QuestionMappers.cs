using System.Linq;

namespace QuizDesigner.Services.Queries
{
    public static class QuestionMappers
    {
        public static IQueryable<QuestionDto> Map(this IQueryable<Question> source)
        {
            return source.Select(x => new QuestionDto
            {
                Id = x.Id,
                Text = x.Text, 
                Tag = x.Tag,
                CreatedOn = x.CreatedOn,
                AnswerCollection = x.Answers.Select(y => new AnswerDto
                {
                    Text = y.Text,
                    IsCorrect = y.IsCorrect
                }).ToList()
            });
        }
    }
}

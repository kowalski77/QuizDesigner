using System;
using System.Collections.Generic;
using System.Linq;
using QuizDesigner.Services;

namespace QuizDesigner.Blazor.App.ViewModels
{
    public static class QuestionsMappers
    {
        public static IEnumerable<Question> ToQuestionCollection(this IEnumerable<QuestionViewModel> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Select(x => x.ToQuestion());
        }

        private static Question ToQuestion(this QuestionViewModel source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new Question
            {
                Text = source.Text,
                Tag = source.Tag
            };
        }
    }
}
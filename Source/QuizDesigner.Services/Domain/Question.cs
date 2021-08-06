using System;
using System.Collections.Generic;
using System.Linq;
using Arch.Utils.DomainDriven;
using Arch.Utils.Functional.Results;

namespace QuizDesigner.Services.Domain
{
    public class Question : Entity, IAggregateRoot
    {
        private readonly List<Answer> answers = new();

        public Question(string text, string? tag)
        {
            this.Text = text;
            this.Tag = tag;
        }

        public string? Text { get; private set; }

        public string? Tag { get; private set; }

        public IEnumerable<Answer> Answers => this.answers;

        public static Result CanAddAnswers(IReadOnlyList<Answer> answerCollection)
        {
            if (answerCollection == null)
            {
                throw new ArgumentNullException(nameof(answerCollection));
            }

            return answerCollection switch
            {
                var x when x.Count < 2 => Result.Fail(nameof(answerCollection),
                    "At least, two answers are needed for each question"),
                var x when x.Count(y => y.IsCorrect) != 1 => Result.Fail(nameof(answerCollection),
                    "Only one answer must be marked as correct"),
                _ => Result.Ok()
            };
        }

        public void AddAnswers(IReadOnlyList<Answer> answerCollection)
        {
            var result = CanAddAnswers(answerCollection);
            if (result.Failure)
            {
                throw new InvalidOperationException($"Cannot add answers due to: {result.Error}");
            }

            this.answers.AddRange(answerCollection);
        }
    }
}

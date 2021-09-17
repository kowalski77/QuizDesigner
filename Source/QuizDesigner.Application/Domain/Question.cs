using System;
using System.Collections.Generic;

namespace QuizDesigner.Application
{
    public class Question : Base
    {
        private readonly List<Answer> answerCollection = new();

        public Question(string text, string tag, Difficulty difficulty)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(tag)) throw new ArgumentNullException(nameof(tag));

            this.Text = text;
            this.Tag = tag;
            this.Difficulty = difficulty;
        }

        public string Text { get; private set; }

        public string Tag { get; private set; }

        public Difficulty Difficulty { get; private set; }

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public IEnumerable<Answer> Answers => this.answerCollection;

        public void Update(string text, string tag, Difficulty timeout)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(tag)) throw new ArgumentNullException(nameof(tag));

            this.Text = text;
            this.Tag = tag;
            this.Difficulty = timeout;
        }

        public void AddAnswers(IEnumerable<Answer> answers)
        {
            if (answers == null) throw new ArgumentNullException(nameof(answers));

            this.answerCollection.AddRange(answers);
        }
    }
}

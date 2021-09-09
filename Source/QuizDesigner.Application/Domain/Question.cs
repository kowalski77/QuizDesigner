using System;
using System.Collections.Generic;

namespace QuizDesigner.Application
{
    public class Question : Base
    {
        private readonly List<Answer> answerCollection = new();

        public Question(string text, string tag)
        {
            if(string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            if(string.IsNullOrEmpty(tag)) throw new ArgumentNullException(nameof(tag));

            this.Text = text;
            this.Tag = tag;
        }

        public string Text { get; private set; }

        public string Tag { get; private set; }

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public IEnumerable<Answer> Answers => this.answerCollection;

        public void SetText(string text)
        {
            if(string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            this.Text = text;
        }

        public void SetTag(string tag)
        {
            if(string.IsNullOrEmpty(tag)) throw new ArgumentNullException(nameof(tag));
            this.Tag = tag;
        }

        public void AddAnswers(IEnumerable<Answer> answers)
        {
            if (answers == null) throw new ArgumentNullException(nameof(answers));

            this.answerCollection.AddRange(answers);
        }
    }
}

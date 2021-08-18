using System;
using System.Collections.Generic;
using Arch.Utils.DomainDriven;

namespace QuizDesigner.Services
{
    public class Question : Entity, IAggregateRoot
    {
        public Question(string text, string tag)
        {
            this.Text = string.IsNullOrEmpty(text) ? throw new ArgumentNullException(nameof(text)) : text;
            this.Tag = string.IsNullOrEmpty(tag) ? throw new ArgumentNullException(nameof(tag)) : tag;
            this.CreatedOn = DateTime.UtcNow;
            this.Answers = new List<Answer>();
        }

        public string Text { get; private set; }

        public string Tag { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public ICollection<Answer> Answers { get; private set; }

        public void AddAnswers(IEnumerable<Answer> answerCollection)
        {
            this.Answers = new List<Answer>(answerCollection);
        }

        public void SetText(string text)
        {
            this.Text = string.IsNullOrEmpty(text) ? throw new ArgumentNullException(nameof(text)) : text;
        }

        public void SetTag(string tag)
        {
            this.Tag = string.IsNullOrEmpty(tag) ? throw new ArgumentNullException(nameof(tag)) : tag;
        }
    }
}

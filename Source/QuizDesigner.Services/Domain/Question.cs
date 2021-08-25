using System;
using System.Collections.Generic;
using Arch.Utils.DomainDriven;

namespace QuizDesigner.Services
{
    public class Question : Entity, IAggregateRoot
    {
        public string Text { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public ICollection<Answer> Answers { get; private set; } = new List<Answer>();

        //TODO: this is not used, how to hide it in a many to many relationship?
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

        public void AddAnswers(IEnumerable<Answer> answerCollection)
        {
            this.Answers = new List<Answer>(answerCollection);
        }

        public void Remove()
        {
            this.SoftDeleted = true;
        }
    }
}

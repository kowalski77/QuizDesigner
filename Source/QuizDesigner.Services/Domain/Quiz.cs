using System;
using System.Collections.Generic;
using Arch.Utils.DomainDriven;

namespace QuizDesigner.Services
{
    public class Quiz : Entity, IAggregateRoot
    {
        public Quiz(string name, string examName, ICollection<Question> questions)
        {
            this.Name = name;
            this.ExamName = examName;
            this.Questions = questions;
            this.CreatedOn = DateTime.UtcNow;
            this.Draft = false;
        }

        public string Name { get; set; }

        public string ExamName { get; set; }

        public DateTime CreatedOn { get; private set; }

        public bool Draft { get;  set; }

        public ICollection<Question> Questions { get; set; }
    }
}
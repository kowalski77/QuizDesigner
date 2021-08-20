using System;
using System.Collections.Generic;
using Arch.Utils.DomainDriven;

namespace QuizDesigner.Services
{
    public class Quiz : Entity, IAggregateRoot
    {
        public Quiz(string name, string examName, 
            DateTime createdOn, ICollection<Question> questions)
        {
            this.Name = name;
            this.ExamName = examName;
            this.CreatedOn = createdOn;
            this.Questions = questions;
            this.Draft = false;
        }

        public string Name { get; private set; }

        public void SetName(string newName)
        {
            this.Name = newName;
        }

        public string ExamName { get; set; }

        public DateTime CreatedOn { get; private set; }

        public bool Draft { get;  set; }

        public ICollection<Question> Questions { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Arch.Utils.DomainDriven;

namespace QuizDesigner.Services
{
    public class Quiz : Entity, IAggregateRoot
    {
        public string Name { get; set; } = string.Empty;

        public string ExamName { get; set; } = string.Empty;

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public bool Draft { get; set; } = true;

        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public void Remove()
        {
            this.SoftDeleted = true;
        }
    }
}
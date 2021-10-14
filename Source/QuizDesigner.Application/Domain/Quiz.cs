using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizDesigner.Application
{
    public class Quiz : Base
    {
        private readonly List<QuizQuestion> quizQuestionCollection = new();

        public Quiz(string name, string category, string email)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(category)) throw new ArgumentNullException(nameof(category));
            if(string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Category = category;
            this.Email = email;
        }

        public string Name { get; private set; }

        public string Category { get; private set; }

        public string Email { get; private set; }

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public bool IsPublished { get; private set; }

        public IEnumerable<QuizQuestion> QuizQuestionCollection => this.quizQuestionCollection;

        public void Update(string name, string examName, string email)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(examName)) throw new ArgumentNullException(nameof(examName));
            if(string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            this.Name = name;
            this.Category = examName;
            this.Email = email;
        }

        public void AddQuestions(IEnumerable<Guid> questionIdCollection)
        {
            if (questionIdCollection == null) throw new ArgumentNullException(nameof(questionIdCollection));

            this.quizQuestionCollection.AddRange(questionIdCollection.Select(id => 
                new QuizQuestion 
                { 
                    QuizId = this.Id, 
                    QuestionId = id
                }));
        }

        public void SetQuestions(IEnumerable<Guid> questionIdCollection)
        {
            if (questionIdCollection == null) throw new ArgumentNullException(nameof(questionIdCollection));

            this.quizQuestionCollection.Clear();
            this.AddQuestions(questionIdCollection);
        }

        public void SetAsPublished()
        {
            this.IsPublished = true;
        }
    }
}
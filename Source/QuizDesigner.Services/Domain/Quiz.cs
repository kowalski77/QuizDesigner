﻿using System;
using System.Collections.Generic;

namespace QuizDesigner.Services
{
    public class Quiz : Base
    {
        private readonly List<QuizQuestion> quizQuestionCollection = new();

        public Quiz(string name, string examName)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(examName)) throw new ArgumentNullException(nameof(examName));

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.ExamName = examName;
        }

        public string Name { get; private set; }

        public string ExamName { get; private set; }

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public bool IsPublished { get; private set; }

        public IEnumerable<QuizQuestion> QuizQuestionCollection => this.quizQuestionCollection;

        public void Update(string name, string examName)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(examName)) throw new ArgumentNullException(nameof(examName));

            this.Name = name;
            this.ExamName = examName;
        }

        public void AddQuestions(IEnumerable<Guid> questionIdCollection)
        {
            if (questionIdCollection == null) throw new ArgumentNullException(nameof(questionIdCollection));

            foreach (var id in questionIdCollection)
            {
                this.quizQuestionCollection.Add(new QuizQuestion
                {
                    QuizId = this.Id,
                    QuestionId = id
                });
            }
        }

        public void UpdateQuestions(IEnumerable<Guid> questionIdCollection)
        {
            if (questionIdCollection == null) throw new ArgumentNullException(nameof(questionIdCollection));

            this.quizQuestionCollection.Clear();
            this.AddQuestions(questionIdCollection);
        }

        public void Publish()
        {
            this.IsPublished = true;
        }
    }
}
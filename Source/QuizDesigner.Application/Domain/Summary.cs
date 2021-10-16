#pragma warning disable CS8618
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizDesigner.Application
{
    public class Summary : Base
    {
        private readonly List<ExamQuestion> questionsCollection = new();

        private Summary() { }

        public Summary(
            Guid quizId, 
            bool passed, 
            string candidate, 
            IEnumerable<string> correctQuestions, 
            IEnumerable<string> wrongQuestions)
        {
            if(string.IsNullOrEmpty(candidate)) throw new ArgumentNullException(nameof(candidate));

            if (correctQuestions == null)
            {
                throw new ArgumentNullException(nameof(correctQuestions));
            }

            if (wrongQuestions == null)
            {
                throw new ArgumentNullException(nameof(wrongQuestions));
            }

            this.QuizId = quizId;
            this.Passed = passed;
            this.Candidate = candidate;

            this.questionsCollection.AddRange(correctQuestions.Select(x=> new ExamQuestion(x, true)));
            this.questionsCollection.AddRange(wrongQuestions.Select(x=> new ExamQuestion(x, false)));
        }

        public Guid QuizId { get; private set; }

        public bool Passed { get; private set; }

        public string Candidate { get; private set; }

        public IReadOnlyList<ExamQuestion> ExamQuestions => this.questionsCollection;
    }
}
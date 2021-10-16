#pragma warning disable CS8618
using System;

namespace QuizDesigner.Application
{
    public class Exam : Base
    {
        private Exam() { }

        public Exam(Guid examId, Summary summary)
        {
            this.ExamId = examId;
            this.Summary = summary ?? throw new ArgumentNullException(nameof(summary));
        }

        public Guid ExamId { get; private set; }

        public Summary Summary { get; private set; }
    }
}
using System;

namespace QuizDesigner.Application
{
    public class QuizDto
    {
        public Guid Id { get; init; }

        public string? Name { get; init; }

        public string? ExamName { get; init; }

        public bool IsPublished { get; init; }
    }
}
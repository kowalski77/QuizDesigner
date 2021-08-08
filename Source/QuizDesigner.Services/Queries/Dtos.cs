using System;
using System.Collections.Generic;

namespace QuizDesigner.Services.Queries
{
    public class QuestionDto
    {
        public Guid Id { get; init; }

        public string? Text { get; init; }

        public string? Tag { get; init; }

        public IEnumerable<AnswerDto> AnswerCollection { get; init; } = new List<AnswerDto>();
    }

    public class AnswerDto
    {
        public string? Text { get; init; }

        public bool IsCorrect { get; init; }
    }
}
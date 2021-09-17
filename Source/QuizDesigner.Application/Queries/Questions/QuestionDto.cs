using System;
using System.Collections.Generic;

namespace QuizDesigner.Application.Queries.Questions
{
    public class QuestionDto
    {
        public Guid Id { get; init; }

        public string? Text { get; init; }

        public string? Tag { get; init; }

        public DifficultyType DifficultyType { get; init; }

        public DateTime? CreatedOn { get; init; }

        public IEnumerable<AnswerDto> AnswerCollection { get; init; } = new List<AnswerDto>();
    }
}
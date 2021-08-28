using System;
using System.Collections.Generic;

namespace QuizDesigner.Application
{
    public class QuestionDto
    {
        public Guid Id { get; init; }

        public string? Text { get; init; }

        public string? Tag { get; init; }

        public DateTime? CreatedOn { get; init; }

        public IEnumerable<AnswerDto> AnswerCollection { get; init; } = new List<AnswerDto>();
    }
}
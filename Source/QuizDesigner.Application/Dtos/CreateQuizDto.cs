using System;
using System.Collections.Generic;

namespace QuizDesigner.Application
{
    public sealed record CreateQuizDto(string Name, string ExamName, IEnumerable<Guid> QuestionIdCollection);
}
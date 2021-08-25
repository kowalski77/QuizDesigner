using System;
using System.Collections.Generic;

namespace QuizDesigner.Services
{
    public sealed record CreateQuizDto(string Name, string ExamName, IEnumerable<Guid> QuestionIdCollection);
}
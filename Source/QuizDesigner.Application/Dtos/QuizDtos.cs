using System;
using System.Collections.Generic;

namespace QuizDesigner.Application
{
    public sealed record CreateQuizDto(string Name, string ExamName, IEnumerable<Guid> QuestionIdCollection);

    public sealed record UpdateQuizDto(Guid QuizId, string Name, string ExamName, IEnumerable<Guid> QuestionIdCollection);
}
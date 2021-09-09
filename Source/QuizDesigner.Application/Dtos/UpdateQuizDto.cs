using System;
using System.Collections.Generic;

namespace QuizDesigner.Application
{
    public record UpdateQuizDto(Guid QuizId, string Name, string ExamName, IEnumerable<Guid> QuestionIdCollection);
}
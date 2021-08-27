using System;
using System.Collections.Generic;

namespace QuizDesigner.Services
{
    public record UpdateQuizDto(Guid QuizId, string Name, string ExamName, IEnumerable<Guid> QuestionIdCollection);
}
using System;
using System.Collections.Generic;

namespace QuizDesigner.Application.IntegrationEvents
{
    public sealed record QuizCreated(Guid Id, string Name, string Exam, IEnumerable<ExamQuestion> ExamQuestionCollection) : IIntegrationEvent;

    public sealed record ExamQuestion(string Text, IEnumerable<ExamAnswer> ExamAnswerCollection);

    public sealed record ExamAnswer(string Text, bool IsCorrect);
}
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace QuizCreatedEvents
{
    public sealed record QuizCreated(Guid Id, string Name, string Exam, IEnumerable<ExamQuestion> ExamQuestionCollection) : IIntegrationEvent;

    public sealed record ExamQuestion(string Text, IEnumerable<ExamAnswer> ExamAnswerCollection);

    public sealed record ExamAnswer(string Text, bool IsCorrect);
}
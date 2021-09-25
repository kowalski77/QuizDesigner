using System;
using System.Collections.Generic;
using QuizCreatedEvents;

namespace QuizDesigner.Events
{
    public sealed record QuizCreated(Guid Id, string Category, string Exam, IEnumerable<ExamQuestion> ExamQuestionCollection) : IIntegrationEvent;

    public sealed record ExamQuestion(string Text, string Tag, int Difficulty, IEnumerable<ExamAnswer> ExamAnswerCollection);

    public sealed record ExamAnswer(string Text, bool IsCorrect);
}
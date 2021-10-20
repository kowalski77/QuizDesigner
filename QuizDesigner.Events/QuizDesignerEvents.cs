using System;
using System.Collections.Generic;

//TODO: common nuget
namespace QuizDesigner.Events
{
    public sealed record QuizCreated(Guid Id, Guid QuizId, string Exam, string Category, IEnumerable<ExamQuestion> ExamQuestionCollection) : IIntegrationEvent;

    public sealed record ExamQuestion(string Text, string Tag, int Difficulty, IEnumerable<ExamAnswer> ExamAnswerCollection);

    public sealed record ExamAnswer(string Text, bool IsCorrect);

    public sealed record ExamFinished(Guid Id, Summary Summary) : IIntegrationEvent;

    public sealed record Summary(
        Guid QuizId,
        bool Passed,
        IEnumerable<string> CorrectQuestionsCollection,
        IEnumerable<string> WrongQuestionsCollection,
        string Candidate);
}
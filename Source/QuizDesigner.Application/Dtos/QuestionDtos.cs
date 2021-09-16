using System;

namespace QuizDesigner.Application
{
    public sealed record CreateQuestionDto(string Text, string Tag);

    public sealed record UpdateQuestionDto(Guid Id, string Text, string Tag);
}
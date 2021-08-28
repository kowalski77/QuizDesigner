using System;

namespace QuizDesigner.Application
{
    public sealed record UpdateQuestionDto(Guid Id, string Text, string Tag);
}
using System;

namespace QuizDesigner.Services
{
    public sealed record UpdateQuestionDto(Guid Id, string Text, string Tag);
}
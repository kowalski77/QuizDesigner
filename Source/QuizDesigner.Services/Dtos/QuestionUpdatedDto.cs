using System;

namespace QuizDesigner.Services
{
    public sealed record QuestionUpdatedDto(Guid Id, string Text, string Tag);
}
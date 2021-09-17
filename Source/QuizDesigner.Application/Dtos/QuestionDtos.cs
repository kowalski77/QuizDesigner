using System;

namespace QuizDesigner.Application
{
    public sealed record CreateQuestionDto(string Text, string Tag, DifficultyType DifficultyType);

    public sealed record UpdateQuestionDto(Guid Id, string Text, string Tag, DifficultyType DifficultyType);
}
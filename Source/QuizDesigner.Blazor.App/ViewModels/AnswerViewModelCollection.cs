using System;
using System.Collections.Generic;

namespace QuizDesigner.Blazor.App.ViewModels
{
    public class AnswerViewModelCollection : List<AnswerViewModel>
    {
        public Guid QuestionId { get; init; }
    }
}
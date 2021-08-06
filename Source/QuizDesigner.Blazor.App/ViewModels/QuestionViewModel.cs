#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizDesigner.Blazor.App.ViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; }

        public string Text { get; }

        public string Tag { get; }

        public QuestionViewModel(Guid id, string text, string tag)
        {
            this.Id = id;
            this.Text = text;
            this.Tag = tag;
        }

        public bool HasAnswers => this.AnswerViewModelCollection.Any();

        public IEnumerable<AnswerViewModel> AnswerViewModelCollection { get; set; } = new List<AnswerViewModel>();
    }
}
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QuizDesigner.Application;

namespace QuizDesigner.Blazor.App.ViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; init; }

        [Required]
        public string Text { get; init; }

        [Required]
        public string Tag { get; init; }

        [Required] 
        public DifficultyType Difficulty { get; set; }

        public bool HasAnswers => this.AnswerViewModelCollection.Any();

        public IEnumerable<AnswerViewModel> AnswerViewModelCollection { get; set; } = new List<AnswerViewModel>();
    }
}
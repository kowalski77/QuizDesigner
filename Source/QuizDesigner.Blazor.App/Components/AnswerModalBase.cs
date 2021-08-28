using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using QuizDesigner.Application;
using QuizDesigner.Blazor.App.Shared;
using QuizDesigner.Blazor.App.Support;
using QuizDesigner.Blazor.App.ViewModels;

namespace QuizDesigner.Blazor.App.Components
{
    public class AnswerModalBase : ComponentBase
    {
        [CascadingParameter] private MainLayout MainLayout { get; set; }

        [Inject] private INotificationService NotificationService { get; set; }

        [Inject] private IQuestionsRepository QuestionsRepository { get; set; }

        [Inject] private IQuestionsProvider QuestionsProvider { get; set; }

        [Parameter]
        public EventCallback<QuestionViewModel> OnAnswersSet { get; set; }

        protected Collection<AnswerViewModel> AnswerViewModelCollection { get; private set; } =
            new BindingList<AnswerViewModel>
            {
                new(), new(), new(), new()
            };

        private Guid questionId;

        protected int CorrectAnswer { get; set; }

        protected Modal ModalRef { get; set; }

        protected Validations Validations { get; set; }

        public async Task ShowModalAsync(Guid id)
        {
            this.questionId = id;
            this.ResetValues();

            await this.FillAnswersAsync(id).ConfigureAwait(true);

            this.ModalRef.Show();
        }

        protected void ValidateSelectOption(ValidatorEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var selectedValue = Convert.ToInt32(e.Value, CultureInfo.InvariantCulture);

            e.Status = selectedValue switch
            {
                var x when x == 2 && string.IsNullOrEmpty(this.AnswerViewModelCollection[2].Text)
                           || x == 3 && string.IsNullOrEmpty(this.AnswerViewModelCollection[3].Text)
                    => ValidationStatus.Error,
                _ => ValidationStatus.Success
            };
        }

        protected async Task SaveAsync()
        {
            var isValid = this.Validations.ValidateAll();
            if (isValid)
            {
                this.MainLayout.ShowLoader(true);
                var success = await this.SaveAnswersAsync().ConfigureAwait(true);
                if (success)
                {
                    await this.InvokeOnAnswersSet().ConfigureAwait(true);
                }
                
                this.MainLayout.ShowLoader(false);
                this.ModalRef.Hide();
            }
        }

        private async Task FillAnswersAsync(Guid id)
        {
            var answerViewModelCollection = await this.GetAnswersAsync(id).ConfigureAwait(true);
            if (answerViewModelCollection.Any())
            {
                for (var i = answerViewModelCollection.Count; i < 4; i++)
                {
                    answerViewModelCollection.Add(new AnswerViewModel());
                }

                this.AnswerViewModelCollection = new BindingList<AnswerViewModel>(answerViewModelCollection);
                this.CorrectAnswer = answerViewModelCollection.FindIndex(x => x.IsCorrect);
            }
        }

        private async Task<List<AnswerViewModel>> GetAnswersAsync(Guid id)
        {
            var question = await this.QuestionsProvider.GetQuestionAsync(id).ConfigureAwait(true);
            if (!question.TryGetValue(out var questionDto))
            {
                await this.NotificationService.Error($"Cannot retrieve question with id: {this.questionId}", "Storage system error").ConfigureAwait(true);

                return Enumerable.Empty<AnswerViewModel>().ToList();
            }

            var answerViewModelCollection = questionDto.AnswerCollection.Select(x =>
                new AnswerViewModel
                {
                    Text = x.Text,
                    IsCorrect = x.IsCorrect
                }).ToList();

            return answerViewModelCollection;
        }

        private async Task<bool> SaveAnswersAsync()
        {
            var notEmptyAnswers = this.AnswerViewModelCollection.Where(x => !string.IsNullOrEmpty(x.Text)).ToList();

            var answerCollection = notEmptyAnswers.Select(x => new Answer(x.Text)).ToList();
            answerCollection[this.CorrectAnswer].SetAsCorrect(true);

            var result = await this.QuestionsRepository.AddAnswersAsync(this.questionId, answerCollection).ConfigureAwait(true);
            await this.NotificationService.ShowSaveQuestionFeedback(result).ConfigureAwait(true);

            return result.Success;
        }

        private async Task InvokeOnAnswersSet()
        {
            var questionViewModel = new QuestionViewModel
            {
                Id = this.questionId,
                AnswerViewModelCollection = this.AnswerViewModelCollection
            };
            await this.OnAnswersSet.InvokeAsync(questionViewModel).ConfigureAwait(true);
        }

        private void ResetValues()
        {
            this.Validations.ClearAll();
            this.AnswerViewModelCollection[0].Text = string.Empty;
            this.AnswerViewModelCollection[1].Text = string.Empty;
            this.AnswerViewModelCollection[2].Text = string.Empty;
            this.AnswerViewModelCollection[3].Text = string.Empty;
            this.CorrectAnswer = 0;
        }
    }
}
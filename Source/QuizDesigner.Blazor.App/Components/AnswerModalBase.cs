using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Blazorise;
using Microsoft.AspNetCore.Components;
using QuizDesigner.Blazor.App.Shared;
using QuizDesigner.Blazor.App.ViewModels;
using QuizDesigner.Services;
using QuizDesigner.Services.Domain;

namespace QuizDesigner.Blazor.App.Components
{
    public class AnswerModalBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        [CascadingParameter] private MainLayout MainLayout { get; set; }

        [Inject] private INotificationService NotificationService { get; set; }

        [Inject] private IQuestionsRepository QuestionsRepository { get; set; }

        protected Collection<AnswerViewModel> AnswerViewModelCollection { get; } =
            new BindingList<AnswerViewModel> 
            { 
                new(), new(), new(), new()
            };

        private Guid questionId;

        protected int CorrectAnswer { get; set; }

        protected Modal ModalRef { get; set; }

        protected Validations Validations { get; set; }

        public void ShowModal(Guid id)
        {
            this.questionId = id;

            this.ResetValues();
            this.ModalRef.Show();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            this.tokenSource?.Cancel();
            this.tokenSource?.Dispose();
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
                await this.SaveAnswersAsync().ConfigureAwait(true);
                this.ModalRef.Hide();
            }
        }

        private async Task SaveAnswersAsync()
        {
            this.MainLayout.ShowLoader(true);

            var notEmptyAnswers = this.AnswerViewModelCollection.Where(x => !string.IsNullOrEmpty(x.Text)).ToList();

            var answerCollection = notEmptyAnswers.Select(x => new Answer(x.Text, x.IsCorrect));
            var result = await this.QuestionsRepository.AddAnswersAsync(this.questionId, answerCollection, this.tokenSource.Token).ConfigureAwait(true);

            await this.ShowFeedback(result).ConfigureAwait(true);

            this.MainLayout.ShowLoader(false);
        }

        private async Task ShowFeedback(Result result)
        {
            if (result.Success)
            {
                await this.NotificationService.Success("Questions successfully saved!")
                    .ConfigureAwait(true);
            }
            else
            {
                await this.NotificationService.Error("An error occurred while sending questions to the storage system", result.Error)
                    .ConfigureAwait(true);
            }
        }

        private void ResetValues()
        {
            this.Validations.ClearAll();
            this.AnswerViewModelCollection[0].Text = string.Empty;
            this.AnswerViewModelCollection[1].Text = string.Empty;
            this.AnswerViewModelCollection[2].Text = string.Empty;
            this.AnswerViewModelCollection[3].Text = string.Empty;
        }
    }
}
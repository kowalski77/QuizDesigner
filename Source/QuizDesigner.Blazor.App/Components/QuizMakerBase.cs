using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuizDesigner.Application;
using QuizDesigner.Blazor.App.Support;

namespace QuizDesigner.Blazor.App.Components
{
    public class QuizMakerBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        private Guid quizId;

        [Inject] private IDesignerService DesignerService { get; set; }

        [Inject] private IJSRuntime JsRuntime { get; set; }

        [Inject] private INotificationService NotificationService { get; set; }

        protected IEnumerable<string> TagCollection { get; private set; }

        protected Validations Validations { get; set; }

        protected string QuizName { get; set; }

        protected string ExamName { get; set; }

        protected bool IsSaved { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            this.TagCollection = await this.DesignerService.GetTags().ConfigureAwait(true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await this.JsRuntime.InvokeVoidAsync("blazorColumnData.initialize").ConfigureAwait(true);
            }
        }

        protected async Task OnSelectedValueChanged(string tag)
        {
            var questionKeyValueCollection = await this.DesignerService.GetQuestionsAsync(tag, this.tokenSource.Token).ConfigureAwait(true);
            foreach (var (key, value) in questionKeyValueCollection)
            {
                await this.JsRuntime.InvokeVoidAsync("blazorColumnData.addQuestion", key.ToString(), value, tag).ConfigureAwait(true);
            }
            await this.JsRuntime.InvokeVoidAsync("blazorColumnData.showQuestions", tag).ConfigureAwait(true);
        }

        protected async Task OnResetAsync()
        {
            await this.JsRuntime.InvokeVoidAsync("blazorColumnData.reset").ConfigureAwait(true);
            this.quizId = Guid.Empty;
            this.QuizName = string.Empty;
            this.ExamName = string.Empty;
            this.IsSaved = false;
            this.Validations.ClearAll();
        }

        protected async Task OnSaveQuizAsync()
        {
            var isValid = this.Validations.ValidateAll();
            if (!isValid)
            {
                return;
            }

            if (this.quizId == Guid.Empty)
            {
                await this.SaveQuizAsync().ConfigureAwait(true);
            }
            else
            {
                await this.UpdateQuizAsync().ConfigureAwait(true);
            }
        }

        protected async Task OnPublishAsync()
        {
            await this.DesignerService.PublishQuizAsync(this.quizId, this.tokenSource.Token).ConfigureAwait(true);
            await this.NotificationService.Success("Quiz successfully published!").ConfigureAwait(true);
            await this.OnResetAsync().ConfigureAwait(true);
        }

        private async Task SaveQuizAsync()
        {
            var questionIdCollection = await this.JsRuntime.InvokeAsync<List<string>>("blazorColumnData.getSelectedQuestions").ConfigureAwait(true);
            if (!questionIdCollection.Any())
            {
                await this.NotificationService.ShowNoSelectedQuestionsError().ConfigureAwait(true);
                return;
            }

            var draftQuiz = new CreateQuizDto(this.QuizName, this.ExamName, questionIdCollection.Select(Guid.Parse));
            var result = await this.DesignerService.CreateQuizAsync(draftQuiz, this.tokenSource.Token).ConfigureAwait(true);

            await this.NotificationService.Success("Quiz successfully saved!").ConfigureAwait(true);

            this.IsSaved = true;
            this.quizId = result;
        }

        private async Task UpdateQuizAsync()
        {
            var questionIdCollection = await this.JsRuntime.InvokeAsync<List<string>>("blazorColumnData.getSelectedQuestions").ConfigureAwait(true);
            if (!questionIdCollection.Any())
            {
                await this.NotificationService.ShowNoSelectedQuestionsError().ConfigureAwait(true);
                return;
            }

            await this.DesignerService.UpdateQuizAsync(
                new UpdateQuizDto(this.quizId, this.QuizName, this.ExamName, questionIdCollection.Select(Guid.Parse)), 
                this.tokenSource.Token)
                .ConfigureAwait(true);

            await this.NotificationService.Success("Quiz successfully updated!").ConfigureAwait(true);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            this.tokenSource?.Cancel();
            this.tokenSource?.Dispose();
        }
    }
}
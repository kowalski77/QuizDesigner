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

        [Parameter] public string Id { get; set; }

        [Inject] private IQuizDataService QuizDataService { get; set; }

        [Inject] private IQuestionsDataProvider QuestionsProvider { get; set; }

        [Inject] private IQuizDataProvider QuizDataProvider { get; set; }

        [Inject] private IJSRuntime JsRuntime { get; set; }

        [Inject] private INotificationService NotificationService { get; set; }

        protected IEnumerable<string> TagCollection { get; private set; }

        protected Validations Validations { get; set; }

        protected string QuizName { get; set; }

        protected string ExamName { get; set; }

        private Guid ParsedId => this.Id != null ? Guid.Parse(this.Id) : Guid.Empty;

        protected override async Task OnInitializedAsync()
        {
            this.TagCollection = await this.QuestionsProvider.GetTags().ConfigureAwait(true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await this.JsRuntime.InvokeVoidAsync("blazorColumnData.initialize").ConfigureAwait(true);

                if (this.ParsedId != Guid.Empty)
                {
                    await this.LoadQuizAsync().ConfigureAwait(true);
                }
            }
        }

        protected async Task OnSelectedValueChanged(string tag)
        {
            var questionKeyValueCollection = await this.QuestionsProvider.GetQuestionsAsync(tag, this.tokenSource.Token).ConfigureAwait(true);
            foreach (var (key, value) in questionKeyValueCollection)
            {
                await this.JsRuntime.InvokeVoidAsync("blazorColumnData.addQuestion", key.ToString(), value, tag).ConfigureAwait(true);
            }
            await this.JsRuntime.InvokeVoidAsync("blazorColumnData.showQuestionsByTag", tag).ConfigureAwait(true);
        }

        protected async Task OnResetAsync()
        {
            await this.JsRuntime.InvokeVoidAsync("blazorColumnData.reset").ConfigureAwait(true);
            this.Id = Guid.Empty.ToString();
            this.QuizName = string.Empty;
            this.ExamName = string.Empty;
            this.Validations.ClearAll();
        }

        protected async Task OnSaveQuizAsync()
        {
            var isValid = this.Validations.ValidateAll();
            if (!isValid)
            {
                return;
            }

            if (this.ParsedId == Guid.Empty)
            {
                await this.SaveQuizAsync().ConfigureAwait(true);
            }
            else
            {
                await this.UpdateQuizAsync().ConfigureAwait(true);
            }
        }

        private async Task LoadQuizAsync()
        {
            var quiz = await this.QuizDataProvider.GetQuizAsync(this.ParsedId, this.tokenSource.Token).ConfigureAwait(true);

            this.QuizName = quiz.Name;
            this.ExamName = quiz.ExamName;

            this.StateHasChanged();

            foreach (var question in quiz.QuizQuestionCollection)
            {
                await this.JsRuntime.InvokeVoidAsync("blazorColumnData.addQuestion", 
                    question.Question!.Id.ToString(), question.Question.Text, question.Question.Tag)
                    .ConfigureAwait(true);
            }

            await this.JsRuntime.InvokeVoidAsync("blazorColumnData.showQuestions", string.Empty).ConfigureAwait(true);
        }

        private async Task SaveQuizAsync()
        {
            var questionIdCollection = await this.JsRuntime.InvokeAsync<List<string>>("blazorColumnData.getSelectedQuestions").ConfigureAwait(true);
            if (!questionIdCollection.Any())
            {
                await this.NotificationService.ShowNoSelectedQuestionsError().ConfigureAwait(true);
                return;
            }

            var quiz = new Quiz(this.QuizName, this.ExamName);
            quiz.AddQuestions(questionIdCollection.Select(Guid.Parse));

            var result = await this.QuizDataService.CreateAsync(quiz).ConfigureAwait(true);
            if (result.Success)
            {
                await this.NotificationService.Success("Quiz successfully saved!").ConfigureAwait(true);
                this.Id = result.Value.ToString();
            }
            else
            {
                await this.NotificationService.Error(result.Error).ConfigureAwait(false);
            }
        }

        private async Task UpdateQuizAsync()
        {
            var questionIdCollection = await this.JsRuntime.InvokeAsync<List<string>>("blazorColumnData.getSelectedQuestions").ConfigureAwait(true);
            if (!questionIdCollection.Any())
            {
                await this.NotificationService.ShowNoSelectedQuestionsError().ConfigureAwait(true);
                return;
            }

            var quiz = await this.QuizDataProvider.GetAsync(this.ParsedId, this.tokenSource.Token).ConfigureAwait(true);
            quiz.Update(this.QuizName, this.ExamName);
            quiz.SetQuestions(questionIdCollection.Select(Guid.Parse));

            var result = await this.QuizDataService.Update(quiz, this.tokenSource.Token).ConfigureAwait(true);
            if (result.Success)
            {
                await this.QuizDataService.UpdateQuestionsAsync(quiz, this.tokenSource.Token).ConfigureAwait(true);
                await this.NotificationService.Success("Quiz successfully updated!").ConfigureAwait(true);
            }
            else
            {
                await this.NotificationService.Error(result.Error).ConfigureAwait(false);
            }
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
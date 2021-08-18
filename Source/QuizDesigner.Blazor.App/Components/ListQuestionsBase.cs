using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using QuizDesigner.Blazor.App.Services;
using QuizDesigner.Blazor.App.ViewModels;
using QuizDesigner.Services;
using QuizDesigner.Services.Domain;

namespace QuizDesigner.Blazor.App.Components
{
    public class ListQuestionsBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        protected AnswersModal AnswersModal { get; set; }

        protected Validations Validations { get; set; }

        [Inject] private IQuestionsProvider QuestionsProvider { get; set; }

        [Inject] private IQuestionsRepository QuestionsRepository { get; set; }

        [Inject] private INotificationService NotificationService { get; set; }

        protected int TotalQuestions { get; private set; }

        protected Collection<QuestionViewModel> QuestionViewModelCollection { get; private set; }

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

        protected void RefreshQuestion(QuestionViewModel updatedQuestionViewModel)
        {
            var question = this.QuestionViewModelCollection.First(x => x.Id == updatedQuestionViewModel.Id);
            question.AnswerViewModelCollection = updatedQuestionViewModel?.AnswerViewModelCollection;
        }

        protected async Task OnRowInserted(SavedRowItem<QuestionViewModel, Dictionary<string, object>> row)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            var question = new Question(row.Item.Text, row.Item.Tag);
            var result = await this.QuestionsRepository.AddAsync(question).ConfigureAwait(true);
            await this.ShowSaveQuestionFeedback(result).ConfigureAwait(true);
            if (result.Success)
            {
                this.StateHasChanged();
            }
        }

        protected async Task OnReadData(DataGridReadDataEventArgs<QuestionViewModel> arg)
        {
            var questionsQuery = new QueryData<QuestionViewModel>(arg, GetFieldValue).ToQuestionsQuery();
            var paginatedModel = (await this.QuestionsProvider.GetQuestionsAsync(questionsQuery, this.tokenSource.Token).ConfigureAwait(true)).ToPageViewModel();

            this.QuestionViewModelCollection = new Collection<QuestionViewModel>(paginatedModel.Items.ToList());
            this.TotalQuestions = paginatedModel.Total;

            this.StateHasChanged();
        }

        protected async Task OnAnswersButtonClickedAsync(Guid questionId)
        {
            await this.AnswersModal.ShowModalAsync(questionId).ConfigureAwait(true);
        }

        private async Task ShowSaveQuestionFeedback(Result result)
        {
            if (result.Success)
            {
                await this.NotificationService.Success("Question successfully saved!").ConfigureAwait(true);
            }
            else
            {
                await this.NotificationService.Error("An error occurred while saving the question", result.Error)
                    .ConfigureAwait(true);
            }
        }

        private static int GetFieldValue(string searchValue)
        {
            return searchValue switch
            {
                "Text" => 1,
                "Tag" => 2,
                _ => 0
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using QuizDesigner.Blazor.App.Services;
using QuizDesigner.Blazor.App.Support;
using QuizDesigner.Blazor.App.ViewModels;
using QuizDesigner.Services;

namespace QuizDesigner.Blazor.App.Components
{
    public class ListQuestionsBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        protected AnswersModal AnswersModal { get; set; }

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

            var question = new Question
            {
                Text = row.Item.Text,
                Tag = row.Item.Tag
            };

            var result = await this.QuestionsRepository.AddAsync(question, this.tokenSource.Token).ConfigureAwait(true);

            await this.NotificationService.ShowSaveQuestionFeedback(result).ConfigureAwait(true);
        }

        protected async Task OnRowUpdated(SavedRowItem<QuestionViewModel, Dictionary<string, object>> row)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            var questionUpdated = new QuestionUpdatedDto(row.Item.Id, row.Item.Text, row.Item.Tag);
            var result = await this.QuestionsRepository.UpdateAsync(questionUpdated, this.tokenSource.Token).ConfigureAwait(true);

            await this.NotificationService.ShowSaveQuestionFeedback(result).ConfigureAwait(true);
        }

        protected async Task OnRowRemoved(QuestionViewModel questionViewModel)
        {
            if (questionViewModel == null) throw new ArgumentNullException(nameof(questionViewModel));

            var result = await this.QuestionsRepository.RemoveAsync(questionViewModel.Id, this.tokenSource.Token).ConfigureAwait(true);

            await this.NotificationService.ShowRemoveQuestionFeedback(result).ConfigureAwait(true);
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
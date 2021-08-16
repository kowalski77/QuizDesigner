using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using QuizDesigner.Blazor.App.Services;
using QuizDesigner.Blazor.App.ViewModels;
using QuizDesigner.Services;

namespace QuizDesigner.Blazor.App.Components
{
    public class ListQuestionsBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        protected AnswersModal AnswersModal { get; set; }

        [Inject] private IQuestionsProvider QuestionsProvider { get; set; }

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

        protected async Task OnReadData(DataGridReadDataEventArgs<QuestionViewModel> arg)
        {
            var questionsQuery = new QueryData<QuestionViewModel>(arg, GetFieldValue).ToQuestionsQuery();
            var paginatedModel = (await this.QuestionsProvider.GetQuestionsAsync(questionsQuery, this.tokenSource.Token).ConfigureAwait(true)).ToPageViewModel();

            this.QuestionViewModelCollection = new Collection<QuestionViewModel>(paginatedModel.Items.ToList());
            this.TotalQuestions = paginatedModel.Total;

            this.StateHasChanged();
        }

        protected Task OnAnswersButtonClickedAsync(Guid questionId)
        {
            this.AnswersModal.ShowModal(questionId);
            return Task.CompletedTask;
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
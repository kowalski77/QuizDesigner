using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using QuizDesigner.Application;
using QuizDesigner.Blazor.App.Services;
using QuizDesigner.Blazor.App.ViewModels;

namespace QuizDesigner.Blazor.App.Components
{
    public class ListQuizBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        [Inject] private IQuizDataProvider QuizDataProvider { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        protected Collection<QuizViewModel> QuizViewModelsCollection { get; private set; }

        protected int TotalQuizzes { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected async Task OnReadData(DataGridReadDataEventArgs<QuizViewModel> arg)
        {
            var quizzesQuery = new QueryData<QuizViewModel>(arg, GetFieldValue)
                .ToQuizzesQuery();
            var paginatedModel = (await this.QuizDataProvider.GetQuizzesAsync(quizzesQuery, this.tokenSource.Token).ConfigureAwait(true))
                .ToPageViewModel();

            this.QuizViewModelsCollection = new Collection<QuizViewModel>(paginatedModel.Items.ToList());
            this.TotalQuizzes = paginatedModel.Total;

            this.StateHasChanged();
        }

        protected void OnNewClick()
        {
            this.NavigationManager.NavigateTo("/create-quiz");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            this.tokenSource?.Cancel();
            this.tokenSource?.Dispose();
        }

        //TODO: maybe to enum directly?
        private static int GetFieldValue(string searchValue)
        {
            return searchValue switch
            {
                "Name" => 1,
                "ExamName" => 2,
                _ => 0
            };
        }
    }
}
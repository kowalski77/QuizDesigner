using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using QuizDesigner.Blazor.App.Shared;
using QuizDesigner.Blazor.App.Support;
using QuizDesigner.Blazor.App.ViewModels;
using QuizDesigner.Services;

namespace QuizDesigner.Blazor.App.Components
{
    public class LoadQuestionsBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        protected Collection<QuestionViewModel> QuestionViewModelCollection { get; } = new();

        protected bool AreQuestionsAvailable { get; private set; }

        protected string TotalQuestions { get; private set; } = "0 questions";

        [CascadingParameter] protected MainLayout MainLayout { get; set; }

        [Inject] private INotificationService NotificationService { get; set; }

        [Inject] private IQuestionsRepository QuestionsRepository { get; set; }

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

        protected void RemoveQuestion(Guid questionId)
        {
            var questionToRemove = this.QuestionViewModelCollection.First(x => x.Id == questionId);
            this.QuestionViewModelCollection.Remove(questionToRemove);
            this.UpdateTotalQuestions();
        }

        protected async Task SendQuestionsAsync()
        {
            this.MainLayout.ShowLoader(true);

            var result = await this.QuestionsRepository.AddRangeAsync(this.QuestionViewModelCollection.ToQuestionCollection(), this.tokenSource.Token).ConfigureAwait(true);
            if (result.Success)
            {
                this.QuestionViewModelCollection.Clear();
                this.UpdateTotalQuestions();
                await this.NotificationService.Success("Questions successfully saved!").ConfigureAwait(true);
            }
            else
            {
                await this.NotificationService.Error("An error occurred while sending questions to the storage system", result.Error).ConfigureAwait(true);
            }

            this.MainLayout.ShowLoader(false);
        }

        protected void DeleteQuestions()
        {
            this.QuestionViewModelCollection.Clear();
            this.UpdateTotalQuestions();
        }

        protected async Task TryLoadQuestionsAsync(FileChangedEventArgs arg)
        {
            if (arg == null) throw new ArgumentNullException(nameof(arg));

            var file = arg.Files.FirstOrDefault();
            if (file == null)
            {
                return;
            }

            await foreach (var question in FileLineReader.ReadQuestionsAsync(file))
            {
                this.QuestionViewModelCollection.Add(question);
            }

            await this.NotificationService.Success($"Successfully loaded {this.QuestionViewModelCollection.Count} questions.").ConfigureAwait(true);

            this.UpdateTotalQuestions();
        }

        private void UpdateTotalQuestions()
        {
            this.TotalQuestions = $"{this.QuestionViewModelCollection.Count} questions";
            this.AreQuestionsAvailable = this.QuestionViewModelCollection.Count > 0;
        }
    }
}
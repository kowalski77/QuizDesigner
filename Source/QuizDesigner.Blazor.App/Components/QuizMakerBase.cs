using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuizDesigner.Blazor.App.ViewModels;
using QuizDesigner.Services;

namespace QuizDesigner.Blazor.App.Components
{
    public class QuizMakerBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        [Inject] private IDesignerService DesignerService { get; set; }

        [Inject] private IJSRuntime JsRuntime { get; set; }

        [Inject] private INotificationService NotificationService { get; set; }

        protected IEnumerable<string> TagCollection { get; private set; }

        protected Validations Validations { get; set; }

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
        }

        protected async Task OnSaveAsync()
        {
            var isValid = this.Validations.ValidateAll();
            if (!isValid)
            {
                return;
            }

            var result = await this.JsRuntime.InvokeAsync<List<string>>("blazorColumnData.getSelectedQuestions").ConfigureAwait(true);
            if (!result.Any())
            {
                await this.NotificationService.Error("Please, drop questions to the right box to save the quiz", "Quiz empty").ConfigureAwait(true);
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
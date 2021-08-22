﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using QuizDesigner.Services;

namespace QuizDesigner.Blazor.App.Components
{
    public class QuizMakerBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource tokenSource = new();

        [Inject] private IDesignerService DesignerService { get; set; }

        protected IEnumerable<string> TagCollection { get; private set; }

        protected string SelectedListValue { get; set; } = string.Empty;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override async Task OnInitializedAsync()
        {
            this.TagCollection = await this.DesignerService.GetTags().ConfigureAwait(true);
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
    }
}
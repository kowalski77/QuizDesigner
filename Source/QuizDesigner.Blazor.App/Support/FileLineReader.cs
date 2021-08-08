using System;
using System.Collections.Generic;
using System.IO;
using Blazorise;
using QuizDesigner.Blazor.App.ViewModels;

namespace QuizDesigner.Blazor.App.Support
{
    public static class FileLineReader
    {
        private const string DefaultTag = "General";

        public static async IAsyncEnumerable<QuestionViewModel> ReadQuestionsAsync(IFileEntry file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            string line;
            await using var stream = new MemoryStream();
            await file.WriteToStreamAsync(stream).ConfigureAwait(true);

            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);

            while ((line = await reader.ReadLineAsync().ConfigureAwait(true)) != null)
            {
                yield return new QuestionViewModel
                {
                    Id = Guid.NewGuid(),
                    Tag = DefaultTag,
                    Text = line
                };
            }
        }
    }
}
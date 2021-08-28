using System;

namespace QuizDesigner.Application
{
    public class Answer : Base
    {
        public Answer(string text)
        {
            if(string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            this.Text = text;
        }

        public string Text { get; private set; }

        public bool IsCorrect { get; private set; }

        public void SetAsCorrect(bool isCorrect)
        {
            this.IsCorrect = isCorrect;
        }
    }
}

using Arch.Utils.DomainDriven;

namespace QuizDesigner.Services.Domain
{
    public class Answer : Entity
    {
        protected Answer()
        {
        }

        public Answer(string text, bool isCorrect)
        {
            this.Text = text;
            this.IsCorrect = isCorrect;
        }

        public string? Text { get; private set; }

        public bool IsCorrect { get; private set; }
    }
}

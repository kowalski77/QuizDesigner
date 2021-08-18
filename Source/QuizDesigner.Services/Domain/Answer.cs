using Arch.Utils.DomainDriven;

namespace QuizDesigner.Services
{
    public class Answer : Entity
    {
        protected Answer()
        {
        }

        public Answer(string text)
        {
            this.Text = text;
        }

        public string? Text { get; private set; }

        public bool IsCorrect { get; private set; }

        public void SetAsCorrect(bool isCorrect)
        {
            this.IsCorrect = isCorrect;
        }
    }
}

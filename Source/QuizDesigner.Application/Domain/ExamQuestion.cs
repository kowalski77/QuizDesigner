#pragma warning disable CS8618
namespace QuizDesigner.Application
{
    public class ExamQuestion : Base
    {
        private ExamQuestion() { }

        public ExamQuestion(string text, bool isCorrect)
        {
            this.Text = text;
            this.IsCorrect = isCorrect;
        }

        public string Text { get; private set; }

        public bool IsCorrect { get; private set; }
    }
}
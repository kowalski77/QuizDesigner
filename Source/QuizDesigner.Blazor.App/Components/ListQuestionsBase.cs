//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Components;
//using QuizDesigner.Blazor.App.ViewModels;

//namespace QuizDesigner.Blazor.App.Components
//{
//    public class ListQuestionsBase : ComponentBase
//    {
//        protected AnswersModal AnswersModal;

//        [Inject] private IQuestionManager QuestionManager { get; set; }

//        protected int TotalQuestions { get; private set; }

//        protected QuestionViewModel[] QuestionViewModelCollection { get; private set; }

//        protected async Task OnReadData(DataGridReadDataEventArgs<QuestionViewModel> arg)
//        {
//            var questionQuery = new QueryData<QuestionViewModel>(arg, GetFieldValue);
//            var pageViewModel = await this.QuestionManager.GetAllQuestionsAsync(questionQuery).ConfigureAwait(true);

//            this.QuestionViewModelCollection = pageViewModel.Items.ToArray();
//            this.TotalQuestions = pageViewModel.Total;

//            this.StateHasChanged();
//        }

//        protected Task OnAnswersButtonClickedAsync(Guid questionId)
//        {
//            this.AnswersModal.ShowModal(questionId);
//            return Task.CompletedTask;
//        }

//        private static int GetFieldValue(string searchValue)
//        {
//            return searchValue switch
//            {
//                "Text" => 1,
//                "Tag" => 2,
//                _ => 0
//            };
//        }
//    }
//}
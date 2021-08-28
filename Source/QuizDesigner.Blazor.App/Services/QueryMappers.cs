using System;
using System.Linq;
using QuizDesigner.Application;
using QuizDesigner.Application.Queries;
using QuizDesigner.Blazor.App.ViewModels;

namespace QuizDesigner.Blazor.App.Services
{
    public static class QueryMappers
    {
        public static QuestionsQuery ToQuestionsQuery(this QueryData<QuestionViewModel> queryData)
        {
            if (queryData == null) throw new ArgumentNullException(nameof(queryData));

            return new QuestionsQuery
            {
                FilterByOptions = (FilterByOptions)queryData.FilterField,
                SortByOptions = (SortByOptions)queryData.SortField,
                FilterValue = queryData.FilterValue,
                AscendingSort = queryData.AscendingSort,
                PageSize = queryData.PageSize,
                PageNumber = queryData.Page
            };
        }

        public static PageViewModel<QuestionViewModel> ToPageViewModel(this IPaginatedModel<QuestionDto> paginatedModel)
        {
            if (paginatedModel == null) throw new ArgumentNullException(nameof(paginatedModel));

            return new PageViewModel<QuestionViewModel>
            {
                Total = paginatedModel.Total,
                Items = paginatedModel.Items.Select(x => new QuestionViewModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    Tag = x.Tag,
                    AnswerViewModelCollection = x.AnswerCollection.Select(y => new AnswerViewModel
                    {
                        Text = y.Text,
                        IsCorrect = y.IsCorrect
                    })
                })
            };
        }
    }
}
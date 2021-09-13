using System;
using System.Linq;
using QuizDesigner.Application;
using QuizDesigner.Application.Queries;
using QuizDesigner.Application.Queries.Quizzes;
using QuizDesigner.Blazor.App.ViewModels;

namespace QuizDesigner.Blazor.App.Services
{
    public static class QuizMappers
    {
        // TODO: DRY with ToQuestionsQuery
        public static QuizzesQuery ToQuizzesQuery(this QueryData<QuizViewModel> queryData)
        {
            if (queryData == null)
            {
                throw new ArgumentNullException(nameof(queryData));
            }

            return new QuizzesQuery
            {
                FilterByOptions = (FilterByOptions)queryData.FilterField,
                SortByOptions = (SortByOptions)queryData.SortField,
                FilterValue = queryData.FilterValue,
                AscendingSort = queryData.AscendingSort,
                PageSize = queryData.PageSize,
                PageNumber = queryData.Page
            };
        }

        public static PageViewModel<QuizViewModel> ToPageViewModel(this IPaginatedModel<QuizDto> paginatedModel)
        {
            if (paginatedModel == null)
            {
                throw new ArgumentNullException(nameof(paginatedModel));
            }

            return new PageViewModel<QuizViewModel>
            {
                Total = paginatedModel.Total,
                Items = paginatedModel.Items.Select(x=> new QuizViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ExamName = x.ExamName,
                    Published = x.IsPublished
                })
            };
        }
    }
}
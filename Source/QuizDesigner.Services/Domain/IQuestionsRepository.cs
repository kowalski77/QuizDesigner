﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Results;

namespace QuizDesigner.Services.Domain
{
    public interface IQuestionsRepository
    {
        Task<Result> AddRangeAsync(IEnumerable<Question> questions, CancellationToken cancellationToken = default);

        Task<Result> AddAnswersAsync(Guid questionId, IEnumerable<Answer> answerCollection, CancellationToken cancellationToken = default);
    }
}
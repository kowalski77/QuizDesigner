﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Application
{
    public interface IQuizDataProvider
    {
        Task<Quiz> GetAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Quiz> GetQuizWithQuestionsAndAnswersAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
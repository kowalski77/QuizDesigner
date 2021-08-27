using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizDesigner.Services;

namespace QuizDesigner.Persistence.Configurations
{
    public class QuizQuestionEntityTypeConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.HasKey(x => new
            {
                x.QuizId, 
                x.QuestionId
            });
        }
    }
}
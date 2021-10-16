using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizDesigner.Application;

namespace QuizDesigner.Persistence.Configurations
{
    public class ExamQuestionEntityTypeConfiguration : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Text).IsRequired();
            builder.HasQueryFilter(x => !x.SoftDeleted);
        }
    }
}
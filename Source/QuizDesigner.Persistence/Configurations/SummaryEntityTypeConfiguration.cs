using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizDesigner.Application;

namespace QuizDesigner.Persistence.Configurations
{
    public class SummaryEntityTypeConfiguration : IEntityTypeConfiguration<Summary>
    {
        public void Configure(EntityTypeBuilder<Summary> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Candidate).IsRequired();
            builder.HasQueryFilter(x => !x.SoftDeleted);
        }
    }
}
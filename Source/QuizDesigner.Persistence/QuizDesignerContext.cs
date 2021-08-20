using System;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Persistence.Configurations;
using QuizDesigner.Services;

namespace QuizDesigner.Persistence
{
    public sealed class QuizDesignerContext : DbContext
    {
        public QuizDesignerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Question>? Questions { get; set; }

        public DbSet<Quiz>? Quizzes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuestionEntityTypeConfiguration).Assembly);
        }
    }
}
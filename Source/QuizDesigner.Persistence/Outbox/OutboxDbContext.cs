using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application.Services.Outbox;

namespace QuizDesigner.Persistence.Outbox
{
    public class OutboxDbContext : DbContext
    {
        public OutboxDbContext(DbContextOptions<OutboxDbContext> options) : base(options)
        {
        }

        public DbSet<OutboxMessage>? OutboxMessages { get; set; }
    }
}
using System;
using System.Threading.Tasks;
using MassTransit;
using QuizCreatedEvents;

namespace QuizDesigner.MessagesConsumer.TestHarness
{
    public class QuizCreatedConsumer : IConsumer<QuizCreated>
    {
        public Task Consume(ConsumeContext<QuizCreated> context)
        {
            Console.WriteLine("Value: {0}", context.Message);

            return Task.CompletedTask;
        }
    }
}
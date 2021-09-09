using System;

namespace QuizDesigner.Application
{
    public abstract class Base
    {
        public Guid Id { get; set; }

        public bool SoftDeleted { get; set; }
    }
}
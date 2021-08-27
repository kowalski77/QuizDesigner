using System;

namespace QuizDesigner.Services
{
    public abstract class Base
    {
        public Guid Id { get; set; }

        public bool SoftDeleted { get; set; }
    }
}
using System;

namespace Core.Events
{
    public interface IDomainEvent
    {
        DateTime CreatedAt { get; set; }
    }
}

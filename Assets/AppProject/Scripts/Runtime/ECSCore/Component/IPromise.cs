using System.Collections.Generic;

namespace ECSCore
{
    public interface IPromise : IFragment
    {
        bool IsFulfilled { get; set; }
        List<IEvent> Resolve { get; }
    };
}

using System.Collections.Generic;

namespace ECSCore
{
    public enum EPromiseState : byte
    {
        Pending, 
        Fulfilled, 
        Rejected
    };
    
    public interface IPromise : IFragment
    {
        EPromiseState State { get; set; }
        List<IEvent> Resolve { get; }
    };
}

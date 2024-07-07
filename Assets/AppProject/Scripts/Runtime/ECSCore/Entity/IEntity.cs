﻿namespace ECSCore
{
    public interface IEntity
    {
        bool Has<T>() where T : class, IFragment;
        bool TryGet<T>(out T fragment) where T : class, IFragment;
        T Trigger<T>() where T : class, IEvent;
        T Then<T>() where T : class, IPromise;
        void Reject<T>() where T : class, IPromise;
        T Add<T>() where T : class, IFragment;
        void Add<T>(T instance) where T : class, IFragment;
        void Remove<T>() where T : class, IFragment;
    };
}

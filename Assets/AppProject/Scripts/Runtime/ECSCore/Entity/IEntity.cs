namespace ECSCore
{
    public interface IEntity
    {
        bool Has<T>() where T : class, IFragment;
        bool TryGet<T>(out T fragment) where T : class, IFragment;
        T Trigger<T>() where T : class, IEvent;
        T Add<T>() where T : class, IFragment;
        void Remove<T>() where T : class, IFragment;
    };
}

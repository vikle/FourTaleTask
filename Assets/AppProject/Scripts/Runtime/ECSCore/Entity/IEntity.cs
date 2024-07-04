namespace ECSCore
{
    public interface IEntity
    {
        bool Has<T>() where T : class, IFragment;
        bool TryGet<T>(out T fragment) where T : class, IFragment;
        void Trigger<T>() where T : class, IEvent;
        void Add<T>() where T : class, IFragment;
        void Remove<T>() where T : class, IFragment;
    };
}

namespace ECSCore
{
    public interface IContextBinding
    {
        IContextBinding BindEvent<T>() where T : class, IEvent;
        IContextBinding BindSystem<T>() where T : class, ISystem, new();
        IContextBinding Inject(object obj);
    };
}

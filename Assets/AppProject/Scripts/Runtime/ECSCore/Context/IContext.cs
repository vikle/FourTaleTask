namespace ECSCore
{
    public interface IContext
    {
        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);
        ContextEnumerator GetEnumerator();
    };
}

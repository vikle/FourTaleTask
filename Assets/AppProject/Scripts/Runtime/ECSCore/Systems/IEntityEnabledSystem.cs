namespace ECSCore
{
    public interface IEntityEnabledSystem : ISystem
    {
        void OnIEntityEnabled(IEntity entity, IContext context);
    };
}

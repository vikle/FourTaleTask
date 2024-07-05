namespace ECSCore
{
    public interface IEntityDisabledSystem: ISystem
    {
        void OnIEntityDisabled(IEntity entity, IContext context);
    };
}

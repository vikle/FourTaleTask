namespace ECSCore
{
    public interface IUpdateSystem : ISystem
    {
        void OnUpdate(IContext context);
    };
}



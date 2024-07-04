namespace ECSCore
{
    public interface IContextRuntime
    {
        void Init();
        void OnStart();
        void OnUpdate();
    };
}

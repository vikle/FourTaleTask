namespace ECSCore
{
    public sealed class PromiseCollector<T> : IUpdateSystem where T : class, IPromise
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out T promise)) continue;
                if (!promise.IsFulfilled) continue;
                
                entity.Remove<T>();

                var resolve = promise.Resolve;

                for (int i = 0, i_max = resolve.Count; i < i_max; i++)
                {
                    entity.Add(resolve[i]);
                }
                
                promise.IsFulfilled = false;
                resolve.Clear();
            }
        }
    };
}

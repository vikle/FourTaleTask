using System.Collections.Generic;

namespace ECSCore
{
    public sealed class EventCollector<T> : IUpdateSystem where T : class, IEvent
    {
        public void OnUpdate()
        {
            foreach (var entity in ECSEngine.GetEntitiesEnumerator())
            {
                if (entity.Has<T>())
                {
                    entity.Remove<T>();
                }
            }
        }
    };
}

using System;
using System.Collections.Generic;

namespace ECSCore
{
    public sealed class Entity : IEntity
    {
        readonly Dictionary<Type, IFragment> m_fragmentsMap = new(8);
        
        public bool Has<T>() where T : class, IFragment
        {
            return m_fragmentsMap.ContainsKey(typeof(T));
        }

        public T Trigger<T>() where T : class, IEvent
        {
            return Add<T>();
        }
        
        public T Then<T>() where T : class, IPromise
        {
            return Add<T>();
        }

        public void Reject<T>() where T : class, IPromise
        {
            if (TryGet(out T promise))
            {
                promise.State = EPromiseState.Rejected;
            }
        }
        
        public T Add<T>() where T : class, IFragment
        {
            var type = typeof(T);
            
            if (m_fragmentsMap.TryGetValue(type, out var raw) && raw is T instance)
            {
                return instance;
            }
            
            instance = FragmentFactory.GetInstance<T>();
            m_fragmentsMap[type] = instance;
            
            return instance;
        }

        public void Add<T>(T instance) where T : class, IFragment
        {
            if (instance == null) return;
            var type = instance.GetType();
            m_fragmentsMap[type] = instance;
        }

        public bool TryGet<T>(out T fragment) where T : class, IFragment
        {
            var type = typeof(T);
            
            if (m_fragmentsMap.TryGetValue(type, out var raw) && raw is T instance)
            {
                fragment = instance;
                return true;
            }

            fragment = default;
            return false;
        }
        
        public void Remove<T>() where T : class, IFragment
        {
            var type = typeof(T);

            if (!m_fragmentsMap.TryGetValue(typeof(T), out var fragment)) return;
            if (fragment is not T instance) return;

            FragmentFactory.Release(instance);
            m_fragmentsMap.Remove(type);
        }
        
        public void Remove<T>(T instance) where T : class, IFragment
        {
            if (instance == null) return;
            var type = instance.GetType();
            m_fragmentsMap.Remove(type);
        }

        public void Dispose()
        {
            EntityFactory.Release(this);
            
            foreach (var instance in m_fragmentsMap.Values)
            {
                FragmentFactory.Release(instance);
            }
            
            m_fragmentsMap.Clear();
        }
    };
}

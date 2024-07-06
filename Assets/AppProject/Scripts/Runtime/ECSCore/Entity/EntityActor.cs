using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECSCore
{
    [DisallowMultipleComponent]
    public sealed class EntityActor : MonoBehaviour, IEntity
    {
        readonly Dictionary<Type, IFragment> m_fragmentsMap = new(8);
        EntityActorComponent[] m_attachedComponents;

        void Awake()
        {
            m_attachedComponents = GetComponents<EntityActorComponent>();
        }

        void OnEnable()
        {
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                m_fragmentsMap[instance.GetType()] = instance;
            }

            ECSEngine.Context.AddEntity(this);
        }

        void OnDisable()
        {
            ECSEngine.Context.RemoveEntity(this);
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                m_fragmentsMap.Remove(instance.GetType());
            }
            
            foreach (var instance in m_fragmentsMap.Values)
            {
                FragmentFactory.Release(instance);
            }
            
            m_fragmentsMap.Clear();
        }

        public bool Has<T>() where T : class, IFragment
        {
            return m_fragmentsMap.ContainsKey(typeof(T));
        }

        public bool TryGet<T>(out T fragment) where T : class, IFragment
        {
            if (m_fragmentsMap.TryGetValue(typeof(T), out var raw) && raw is T instance)
            {
                fragment = instance;
                return true;
            }

            fragment = default;
            return false;
        }

        public T Trigger<T>() where T : class, IEvent
        {
            return Add<T>();
        }
        
        public T Then<T>() where T : class, IPromise
        {
            return Add<T>();
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
            m_fragmentsMap[instance.GetType()] = instance;
        }

        public void Remove<T>() where T : class, IFragment
        {
            var type = typeof(T);

            if (!m_fragmentsMap.TryGetValue(typeof(T), out var fragment)) return;
            if (fragment is not T instance) return;

            FragmentFactory.Release(instance);
            m_fragmentsMap.Remove(type);
        }
    };
}

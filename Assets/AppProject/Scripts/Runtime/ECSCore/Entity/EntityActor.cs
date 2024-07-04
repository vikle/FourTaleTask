using System;
using System.Collections.Generic;
using UnityEngine;
using ECSCore.Tools;

namespace ECSCore
{
    [DisallowMultipleComponent]
    public sealed class EntityActor : MonoBehaviour, IEntity
    {
        readonly Dictionary<Type, IFragment> m_components = new(8);

        void OnEnable()
        {
            ECSEngine.Context.AddEntity(this);
        }

        void OnDisable()
        {
            ECSEngine.Context.RemoveEntity(this);
        }

        public bool Has<T>() where T : class, IFragment
        {
            return m_components.ContainsKey(typeof(T));
        }

        public bool TryGet<T>(out T fragment) where T : class, IFragment
        {
            if (m_components.TryGetValue(typeof(T), out var raw) && raw is T instance)
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

        public T Add<T>() where T : class, IFragment
        {
            var type = typeof(T);
            
            if (m_components.TryGetValue(type, out var raw) && raw is T instance)
            {
                return instance;
            }
            
            instance = FragmentFactory.GetInstance<T>();
            
            m_components[type] = instance;

            return instance;
        }

        public void Remove<T>() where T : class, IFragment
        {
            var type = typeof(T);

            if (m_components.TryGetValue(typeof(T), out var fragment) == false) return;
            if (fragment is not T instance) return;

            FragmentFactory.Release(instance);
            m_components.Remove(type);
        }
        
        public void Add<T>(T instance) where T : EntityActorComponent
        {
            m_components[typeof(T)] = instance;
        }
        
        public void Remove<T>(T instance) where T : EntityActorComponent
        {
            m_components.Remove(instance.GetType());
        }
    };
}

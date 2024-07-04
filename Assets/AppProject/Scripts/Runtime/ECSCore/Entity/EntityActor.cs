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
            ECSEngine.AddEntity(this);
        }

        void OnDisable()
        {
            ECSEngine.RemoveEntity(this);
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

        public void Trigger<T>() where T : class, IEvent
        {
            Add<T>();
        }

        public void Add<T>() where T : class, IFragment
        {
            var type = typeof(T);
            if (m_components.ContainsKey(type)) return;
            m_components[type] = FragmentFactory.GetInstance<T>();
        }

        public void Remove<T>() where T : class, IFragment
        {
            var type = typeof(T);

            if (m_components.TryGetValue(typeof(T), out var fragment) == false) return;
            if (fragment is not T instance) return;

            FragmentFactory.Release(instance);
            m_components.Remove(type);
        }
    };
}

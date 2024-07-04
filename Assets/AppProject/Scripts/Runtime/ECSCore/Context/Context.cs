using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace ECSCore
{
    public sealed class Context : IContext, IContextBinding, IContextRuntime
    {
        readonly List<IEntity> m_entities = new(4);
        readonly List<ISystem> m_allSystems = new(8);
        readonly List<IUpdateSystem> m_updateSystems = new(8);
        ArrayList m_injectionsCache = new(8);
        
        public void AddEntity(IEntity entity)
        {
            if (m_entities.Contains(entity) == false)
            {
                m_entities.Add(entity);
            }
        }
    
        public void RemoveEntity(IEntity entity)
        {
            int entity_id = m_entities.IndexOf(entity);
        
            if (entity_id > -1)
            {
                m_entities.RemoveAt(entity_id);
            }
        }

        public IContextBinding BindEvent<T>() where T : class, IEvent
        {
            return BindSystem<EventCollector<T>>();
        }
    
        public IContextBinding BindSystem<T>() where T : class, ISystem, new()
        {
            var bin_system = new T();
        
            m_allSystems.Add(bin_system);
        
            switch (bin_system)
            {
                case IUpdateSystem update_system: 
                    m_updateSystems.Add(update_system);
                    break;
            }

            return this;
        }

        public IContextBinding Inject(object data)
        {
            if (m_injectionsCache.Contains(data) == false)
            {
                m_injectionsCache.Add(data);
            }
            
            return this;
        }

        public void Init()
        {
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            const BindingFlags k_binding_flags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                var system = m_allSystems[i];
                var system_fields = system.GetType().GetFields(k_binding_flags);

                for (int j = 0, j_max = system_fields.Length; j < j_max; j++)
                {
                    var field = system_fields[j];
                    var field_type = field.FieldType;
                    
                    for (int k = 0, k_max = m_injectionsCache.Count; k < k_max; k++)
                    {
                        object injection = m_injectionsCache[k];
                        if (field_type.IsInstanceOfType(injection) == false) continue;
                        field.SetValue(system, injection);
                        break;
                    }
                }
            }
            
            m_injectionsCache.Clear();
            m_injectionsCache = null;
        }
        
        public void OnStart()
        {
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                if (m_allSystems[i] is IStartSystem start_system)
                {
                    start_system.OnStart(this);
                }
            }
        }
    
        public void OnUpdate()
        {
            for (int i = 0, i_max = m_updateSystems.Count; i < i_max; i++)
            {
                m_updateSystems[i].OnUpdate(this);
            }
        }

        public ContextEnumerator GetEnumerator()
        {
            return new(m_entities);
        }
    };
}

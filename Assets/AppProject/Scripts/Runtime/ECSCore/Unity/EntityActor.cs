using UnityEngine;

namespace ECSCore
{
    [DisallowMultipleComponent]
    public sealed class EntityActor : MonoBehaviour
    {
        public IEntity Entity { get; private set; }
        
        EntityActorComponent[] m_attachedComponents;

        void Awake()
        {
            InitAttachedComponents();
        }

        void OnEnable()
        {
            InitEntity();
        }

        void OnDisable()
        {
            DisposeEntity();
        }

        public void InitEntity()
        {
            if (Entity != null) return;
            
            InitAttachedComponents();
            
            var entity = EntityFactory.GetInstance<Entity>();
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                entity.Add(instance);
            }

            ECSEngine.Context.AddEntity(entity);

            Entity = entity;
        }

        public void DisposeEntity()
        {
            var entity = Entity;
            
            if (entity == null) return;

            ECSEngine.Context.RemoveEntity(entity);
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                entity.Remove(instance);
            }
            
            entity.Dispose();
            Entity = null;
        }

        private void InitAttachedComponents()
        {
            m_attachedComponents ??= GetComponents<EntityActorComponent>();
        }
    };
}

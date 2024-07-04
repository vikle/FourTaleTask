using System.Collections.Generic;

namespace ECSCore
{
    public struct ContextEnumerator
    {
        readonly IReadOnlyList<IEntity> m_entities;
        readonly int m_count;
        int m_index;

        public ContextEnumerator(IReadOnlyList<IEntity> entities)
        {
            m_entities = entities;
            m_count = m_entities.Count;
            m_index = -1;
            Current = default;
        }
     
        public IEntity Current { get; private set; }

        public bool MoveNext()
        {
            int count = m_count;
            ref int index = ref m_index;
            var entities = m_entities;
            
            if (++index >= count) return false;

            Current = entities[index];
            return true;
        }
    };
}

using System.Collections.Generic;
using UnityEngine;
using ECSCore;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-1)]
public sealed class ECSEngine : MonoBehaviour
{
    public MonoBehaviour bootstrap;
    
    static readonly List<IEntity> sr_entities = new(4);

    static readonly List<ISystem> sr_allSystems = new(8);
    static readonly List<IUpdateSystem> sr_updateSystems = new(8);

    public static void AddEntity(IEntity entity)
    {
        if (sr_entities.Contains(entity) == false)
        {
            sr_entities.Add(entity);
        }
    }
    
    public static void RemoveEntity(IEntity entity)
    {
        int entity_id = sr_entities.IndexOf(entity);
        
        if (entity_id > -1)
        {
            sr_entities.RemoveAt(entity_id);
        }
    }

    public static void BindEvent<T>() where T : class, IEvent
    {
        BindSystem<EventCollector<T>>();
    }
    
    public static void BindSystem<T>() where T : class, ISystem, new()
    {
        var bin_system = new T();
        
        sr_allSystems.Add(bin_system);
        
        switch (bin_system)
        {
            case IUpdateSystem update_system: 
                sr_updateSystems.Add(update_system);
                break;
        }
    }

    public static void Inject(object obj)
    {
        for (int i = 0, i_max = sr_allSystems.Count; i < i_max; i++)
        {
            
        }
    }

    void Awake()
    {
        if (bootstrap is IECSBootstrap ecs_bootstrap)
        {
            ecs_bootstrap.OnBootstrap();
        }
    }

    void Start()
    {
        for (int i = 0, i_max = sr_allSystems.Count; i < i_max; i++)
        {
            if (sr_allSystems[i] is IStartSystem start_system)
            {
                start_system.OnStart();
            }
        }
    }
    
    void Update()
    {
        for (int i = 0, i_max = sr_updateSystems.Count; i < i_max; i++)
        {
            sr_updateSystems[i].OnUpdate();
        }
    }

    public static ECSEngineEntitiesEnumerator GetEntitiesEnumerator()
    {
        return new(sr_entities);
    }
};

public struct ECSEngineEntitiesEnumerator
{
    readonly IReadOnlyList<IEntity> m_entities;

    public ECSEngineEntitiesEnumerator(IReadOnlyList<IEntity> entities)
    {
        m_entities = entities;
    }
    
    public ECSEngineEnumerator GetEnumerator()
    {
        return new(m_entities);
    }
};

public struct ECSEngineEnumerator
{
    readonly IReadOnlyList<IEntity> m_entities;
    readonly int m_count;
    int m_index;

    public ECSEngineEnumerator(IReadOnlyList<IEntity> entities)
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
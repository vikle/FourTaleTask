﻿using UnityEngine;

namespace ECSCore
{
    [RequireComponent(typeof(EntityActor))]
    public abstract class EntityActorComponent : MonoBehaviour, IComponent
    {
        public EntityActor actor;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (actor == null) actor = GetComponent<EntityActor>();
        }
#endif
        protected virtual void OnEnable()
        {
            actor.Add(this);
        }
        
        protected virtual void OnDisable()
        {
            actor.Remove(this);
        }
    };
}

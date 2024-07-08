﻿namespace ECSCore
{
    public sealed class EntityFactory : ObjectPool<IEntity>
    {
        public static T GetInstance<T>() where T : class, IEntity
        {
            return GetInstanceInternal<T>();
        }
        
        public static void Release<T>(T instance) where T : class, IEntity
        {
            ReleaseInternal(instance);
        }
    };
}

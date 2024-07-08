using System;
using System.Collections.Generic;

namespace ECSCore
{
    public abstract class ObjectPool<TType> where TType : class
    {
        static readonly Dictionary<Type, Stack<TType>> sr_pool = new(8);
        
        protected static TValue GetInstanceInternal<TValue>() where TValue : class, TType
        {
            var pool_type = typeof(TValue);

            TValue instance;
            
            if (sr_pool.TryGetValue(pool_type, out var stack))
            {
                instance = (stack.Count > 0) 
                    ? (TValue)stack.Pop() 
                    : Activator.CreateInstance<TValue>();
            }
            else
            {
                instance = Activator.CreateInstance<TValue>();
                sr_pool[pool_type] = new(8);
            }
            
            return instance;
        }
        
        protected static void ReleaseInternal<TValue>(TValue instance) where TValue : class, TType
        {
            var pool_type = instance.GetType();

            if (sr_pool.TryGetValue(pool_type, out var stack) == false)
            {
                stack = new(8);
                sr_pool[pool_type] = stack;
            }
            
            stack.Push(instance);
        }
    };
}

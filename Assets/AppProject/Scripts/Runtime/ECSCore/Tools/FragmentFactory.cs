using System;
using System.Collections.Generic;

namespace ECSCore.Tools
{
    public static class FragmentFactory
    {
        static readonly Dictionary<Type, Stack<IFragment>> sr_pool = new(8);
        
        public static T GetInstance<T>() where T : class, IFragment
        {
            var pool_type = typeof(T);

            T instance;
            
            if (sr_pool.TryGetValue(pool_type, out var stack))
            {
                instance = (stack.Count > 0) 
                    ? (T)stack.Pop() 
                    : Activator.CreateInstance<T>();
            }
            else
            {
                instance = Activator.CreateInstance<T>();
                sr_pool[pool_type] = new(8);
            }

            instance.OnCreated();
            
            return instance;
        }
        
        public static void Release(IFragment instance)
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

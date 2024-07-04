using System;
using UnityEngine;

namespace ECSCore
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-1)]
    public sealed class ECSEngine : MonoBehaviour
    {
        public static IContext Context { get; private set; }
        static IContextRuntime s_runtime;
        
        public MonoBehaviour bootstrap;
    
        void Awake()
        {
            var context = new Context();
            Context = context;
            s_runtime = context;
            
            if (bootstrap is IECSBootstrap ecs_bootstrap)
            {
                ecs_bootstrap.OnBootstrap(context);
            }
            
            s_runtime.Init();
        }

        void Start()
        {
            s_runtime.OnStart();
        }

        void Update()
        {
            s_runtime.OnUpdate();
        }
    };
}

﻿namespace ECSCore
{
    public interface IStartSystem : ISystem
    {
        void OnStart(IContext context);
    };
}

using System;
using System.Collections.Generic;

public static class EventBus
{
    static readonly Dictionary<string, List<Delegate>> sr_events = new(8);

    public static void Register(string hookName, Action handler)
    {
        var handlers = GetHandlers(hookName);
        handlers.Add(handler);
    }

    public static void Register<TArgs>(string hookName, Action<TArgs> handler)
    {
        var handlers = GetHandlers(hookName);
        handlers.Add(handler);
    }

    public static void Unregister(string hookName, Delegate handler)
    {
        if (!sr_events.TryGetValue(hookName, out var handlers)) return;
        if (!handlers.Remove(handler)) return;
        if (handlers.Count > 0) return;
        sr_events.Remove(hookName);
    }

    public static void Trigger(string hookName)
    {
        var handlers = GetHandlers(hookName);

        for (int i = 0, i_max = handlers.Count; i < i_max; i++)
        {
            if (handlers[i] is Action action)
            {
                action.Invoke();
            }
        }
    }

    public static void Trigger<TArgs>(string hookName, TArgs args)
    {
        var handlers = GetHandlers(hookName);

        for (int i = 0, i_max = handlers.Count; i < i_max; i++)
        {
            if (handlers[i] is Action<TArgs> action)
            {
                action.Invoke(args);
            }
        }
    }

    private static List<Delegate> GetHandlers(string hookName)
    {
        if (sr_events.TryGetValue(hookName, out var handlers))
        {
            return handlers;
        }

        handlers = new();
        sr_events.Add(hookName, handlers);

        return handlers;
    }
};

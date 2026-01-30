using System;
using System.Collections.Generic;

public static class Services
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public static T Get<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;

        throw new Exception($"Service not registered: {typeof(T)}");
    }

    public static object Get(Type type)
    {
        if (_services.TryGetValue(type, out var service))
            return service;

        throw new Exception($"Service not registered: {type}");
    }
}
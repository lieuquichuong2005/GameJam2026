using System.Reflection;
using UnityEngine;

public static class Injector
{
    public static void Inject(object target)
    {
        var fields = target.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            if (!field.IsDefined(typeof(InjectAttribute), inherit: true))
                continue;

            var service = Services.Get(field.FieldType);
            field.SetValue(target, service);
        }
    }
}   
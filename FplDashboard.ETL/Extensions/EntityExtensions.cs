using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FplDashboard.ETL.Extensions;

public static class EntityExtensions
{
    public static void CopyMutablePropertiesFrom<T>(this T target, T source)
    {
        var type = typeof(T);
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Skip keys (by convention or attribute)
            if (property.Name == "Id" || property.GetCustomAttribute<KeyAttribute>() != null)
                continue;

            // Skip navigation properties (collections or complex types except string)
            if (!property.CanWrite || (property.PropertyType.IsClass && property.PropertyType != typeof(string)))
                continue;

            // Skip init-only properties
            var setMethod = property.SetMethod;
            if (setMethod == null)
                continue;
            var isInitOnly = setMethod.ReturnParameter
                .GetRequiredCustomModifiers()
                .Contains(typeof(IsExternalInit));
            if (isInitOnly)
                continue;

            // Copy value
            var value = property.GetValue(source);
            property.SetValue(target, value);
        }
    }
}


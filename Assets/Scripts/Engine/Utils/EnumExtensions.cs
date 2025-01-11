using System;
using System.Linq;

public static class EnumExtensions
{
    public static object GetAssociatedClass(this Enum enumValue, params object[] constructorArgs)
    {
        // Get the type of the enum
        var enumType = enumValue.GetType();

        // Get the field info for the specific enum value
        var fieldInfo = enumType.GetField(enumValue.ToString());

        // Get the custom attribute from the field
        var attribute = fieldInfo
            .GetCustomAttributes(typeof(ClassMappingAttribute), false)
            .FirstOrDefault() as ClassMappingAttribute;

        // Return an instance of the mapped type if it exists
        return attribute != null ? Activator.CreateInstance(attribute.MappedType, constructorArgs) : null;
    }
    public static T GetNext<T>(this T enumValue) where T : Enum
    {
        // Get all values of the enum
        T[] values = (T[])Enum.GetValues(enumValue.GetType());
        // Find the index of the current value
        int currentIndex = Array.IndexOf(values, enumValue);
        // Compute the index of the next value, looping back to zero if at the end
        int nextIndex = (currentIndex + 1) % values.Length;
        // Return the next value
        return values[nextIndex];
    }
}

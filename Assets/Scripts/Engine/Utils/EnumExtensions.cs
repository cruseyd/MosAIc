using System;
using System.Linq;

public static class EnumExtensions
{
    public static object GetAssociatedClass(this Enum enumValue)
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
        return attribute != null ? Activator.CreateInstance(attribute.MappedType) : null;
    }
}

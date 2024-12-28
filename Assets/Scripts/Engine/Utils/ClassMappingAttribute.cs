using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ClassMappingAttribute : Attribute
{
    public Type MappedType { get; }

    public ClassMappingAttribute(Type mappedType)
    {
        MappedType = mappedType;
    }
}

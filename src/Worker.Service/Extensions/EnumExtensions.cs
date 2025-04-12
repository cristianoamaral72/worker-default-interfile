using System.ComponentModel;
using System;

namespace Worker.Service.Extensions;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        return attribute == null ? value.ToString() : attribute.Description;
    }
}
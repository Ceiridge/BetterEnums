namespace BetterEnums {
	internal static class BetterEnumsSources {
		public const string NAMESPACE = "BetterEnumsGen";
		public const string ATTRIBUTE_SHORT = "BetterEnum";
		public const string ATTRIBUTE = ATTRIBUTE_SHORT + "Attribute";
		public const string EXTENSION = "BetterEnumExtensions";

		public static readonly string ATTRIBUTE_SOURCE = $@"
using System;
namespace {NAMESPACE} {{
	[AttributeUsage(AttributeTargets.Enum)]
	public class {ATTRIBUTE} : Attribute {{
	}}
}}";
		public static readonly string ENUM_EXTENSION_SOURCE_START = $@"
using System;
using System.Reflection;
using System.Linq;
namespace {NAMESPACE} {{
	public static class {EXTENSION} {{";

		public const string ENUM_TO_STRING_SOURCE = @"
/// <summary>
/// Returns the enum member identifier name (more performant)
/// </summary>
public static string BetterToString(this {0} @enum) {{
";
		public const string ENUM_VALUES_SOURCE = @"
/// <summary>
/// Returns the respective attribute of the given enum.
/// May return null if the enum does not have that attribute.
/// </summary>
public static {0}? Get{1}(this {2} @enum) {{
";

		public const string ENUM_SWITCH_SOURCE_START = @"	return @enum switch {";
		public const string ENUM_SWITCH_SOURCE_END = @"_ => throw new ArgumentOutOfRangeException(nameof(@enum), @enum, null)
	};
}";
		public const string ENUM_SWITCH_SOURCE_END_NULL = @"_ => null
	};
}";

		public const string ENUM_EXTENSION_SOURCE_END = @"
	}
}";

		public const string ENUM_GET_ATTRIBUTE_METHOD_SOURCE = @"
	// Taken from https://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value
	/// <summary>
	/// Gets an attribute on an enum field value
	/// </summary>
	/// <typeparam name=""T"">The type of the attribute you want to retrieve</typeparam>
	/// <param name=""enumVal"">The enum value</param>
	/// <returns>The attribute of type T that exists on the enum value</returns>
	/// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
	public static T? GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute {
		Type type = enumVal.GetType();
		MemberInfo[] memInfo = type.GetMember(enumVal.ToString());
		object[] attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
		return (attributes.Length > 0) ? (T)attributes[0] : null;
	}";

		public const string ENUM_GET_ATTRIBUTES_METHOD_SOURCE = @"
	private static object[] GetEnumAttributes(Type enumType) {
		return enumType.GetMembers()
			.Where(member => member.MemberType == MemberTypes.Field && ((FieldInfo)member).FieldType == enumType)
			.Select(member => member.GetCustomAttributes(false))
			.SelectMany(attributeList => attributeList)
			.ToArray();
	}";
	}
}

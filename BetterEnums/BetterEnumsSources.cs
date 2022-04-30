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
namespace {NAMESPACE} {{
	public static class {EXTENSION} {{";

		public const string ENUM_TO_STRING_SOURCE = @"
/// <summary>
/// Returns the enum member identifier name (more performant)
/// </summary>
public static string BetterToString(this {0} @enum) {{
	return @enum switch {{
";
		public const string ENUM_TO_STRING_SOURCE_END = @"_ => throw new ArgumentOutOfRangeException(nameof(@enum), @enum, null)
	};
}";

		public const string ENUM_EXTENSION_SOURCE_END = @"
	}
}";
	}
}

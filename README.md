# C# Better Enums Source Generator

A step in making C# enums usable in order to not fall behind Java.\
A source generator that adds extension methods for enums with any attribute. Also adds more performant methods.

Install the [NuGet](https://www.nuget.org/packages/BetterEnums) or [GitHub](https://github.com/Ceiridge?tab=packages&repo_name=CSharp-Better-Enums) package.

## How to use
```csharp
[BetterEnum] // Add this attribute to the enum
public enum ExampleEnum {
	AnExample,
	[ExampleEnumInfo("Hello", 1)] // Add attributes to your enum members
	[Description("Another text")]
	Another,
	[Description("1")]
	One
}
```

The following methods are now available via an extension class:
```csharp
// Faster ToString
ExampleEnum.AnExample.BetterToString();
// Access the value attributes directly
ExampleEnum.Another.GetExampleEnumInfo();
ExampleEnum.Another.GetDescription().Description;
// Programmatically access an attribute (uses slow runtime reflection on each call)
ExampleEnum.One.GetAttributeOfType<DescriptionAttribute>();
```

<details>
<summary>Generated source code for one example enum</summary>

```csharp
[BetterEnum]
public enum ExampleEnum {
	[ExampleEnumInfo("Example Text", 123)]
	[Description("Multiple attributes")]
	Example,
	[ExampleEnumInfo("Test Enum", 5)]
	Test,
	[ExampleEnumInfo("Working enum", 7)]
	[Description("Even more attributes")]
	Working
}
```

```csharp
using System;
using System.Reflection;
namespace BetterEnumsGen {
	public static class BetterEnumExtensions {
		private static readonly BetterEnumsExample.ExampleEnumInfoAttribute? attr_24e6ce3758d72003196ef350428e4c1891570e9e5a2ba641ed73cea79a989ad0;
		private static readonly System.ComponentModel.DescriptionAttribute? attr_e770e50ffdea7a63dab5345238ae5f03a375d5a2e505ccf5f4f6baa9f6873a99;
		private static readonly BetterEnumsExample.ExampleEnumInfoAttribute? attr_2cbdc8a4a473f08b7376601fc61353eb382f266e9d2d2856d0921461c9636c8a;
		private static readonly BetterEnumsExample.ExampleEnumInfoAttribute? attr_5cd26a3daff2aa7ab2f9e7f1fa4e397a29f82e0c4a71e4cecf91aa9eb41c94a2;
		private static readonly System.ComponentModel.DescriptionAttribute? attr_feead0a86a3e9dd9da71d5161b6779d6ea88fc67a76495781fe836422a5e714e;

		/// <summary>
		/// Returns the enum member identifier name (more performant)
		/// </summary>
		public static string BetterToString(this BetterEnumsExample.ExampleEnum @enum) {
			return @enum switch {
				BetterEnumsExample.ExampleEnum.Example => nameof(BetterEnumsExample.ExampleEnum.Example),
				BetterEnumsExample.ExampleEnum.Test => nameof(BetterEnumsExample.ExampleEnum.Test),
				BetterEnumsExample.ExampleEnum.Working => nameof(BetterEnumsExample.ExampleEnum.Working),
				_ => throw new ArgumentOutOfRangeException(nameof(@enum), @enum, null)
			};
		}

		/// <summary>
		/// Returns the respective attribute of the given enum.
		/// May return null if the enum does not have that attribute.
		/// </summary>
		public static BetterEnumsExample.ExampleEnumInfoAttribute? GetExampleEnumInfo(this BetterEnumsExample.ExampleEnum @enum) {
			return @enum switch {
				BetterEnumsExample.ExampleEnum.Example => attr_24e6ce3758d72003196ef350428e4c1891570e9e5a2ba641ed73cea79a989ad0,
				BetterEnumsExample.ExampleEnum.Test => attr_2cbdc8a4a473f08b7376601fc61353eb382f266e9d2d2856d0921461c9636c8a,
				BetterEnumsExample.ExampleEnum.Working => attr_5cd26a3daff2aa7ab2f9e7f1fa4e397a29f82e0c4a71e4cecf91aa9eb41c94a2,
				_ => null
			};
		}


		/// <summary>
		/// Returns the respective attribute of the given enum.
		/// May return null if the enum does not have that attribute.
		/// </summary>
		public static System.ComponentModel.DescriptionAttribute? GetDescription(this BetterEnumsExample.ExampleEnum @enum) {
			return @enum switch {
				BetterEnumsExample.ExampleEnum.Example => attr_e770e50ffdea7a63dab5345238ae5f03a375d5a2e505ccf5f4f6baa9f6873a99,
				BetterEnumsExample.ExampleEnum.Working => attr_feead0a86a3e9dd9da71d5161b6779d6ea88fc67a76495781fe836422a5e714e,
				_ => null
			};
		}

		// Finds all attributes once at runtime
		static BetterEnumExtensions() {
			object[] attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890 = GetEnumAttributes(typeof(BetterEnumsExample.ExampleEnum));
			attr_24e6ce3758d72003196ef350428e4c1891570e9e5a2ba641ed73cea79a989ad0 = 0 >= attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890.Length ? null : attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890[0] as BetterEnumsExample.ExampleEnumInfoAttribute;
			attr_e770e50ffdea7a63dab5345238ae5f03a375d5a2e505ccf5f4f6baa9f6873a99 = 1 >= attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890.Length ? null : attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890[1] as System.ComponentModel.DescriptionAttribute;
			attr_2cbdc8a4a473f08b7376601fc61353eb382f266e9d2d2856d0921461c9636c8a = 2 >= attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890.Length ? null : attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890[2] as BetterEnumsExample.ExampleEnumInfoAttribute;
			attr_5cd26a3daff2aa7ab2f9e7f1fa4e397a29f82e0c4a71e4cecf91aa9eb41c94a2 = 3 >= attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890.Length ? null : attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890[3] as BetterEnumsExample.ExampleEnumInfoAttribute;
			attr_feead0a86a3e9dd9da71d5161b6779d6ea88fc67a76495781fe836422a5e714e = 4 >= attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890.Length ? null : attrs_7299f2836dc3779f856acae2b30cad36ea256b0f96ef137f9a8298c8ad88f890[4] as System.ComponentModel.DescriptionAttribute;
		}

		// Taken from https://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value
		/// <summary>
		/// Gets an attribute on an enum field value
		/// </summary>
		/// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
		/// <param name="enumVal">The enum value</param>
		/// <returns>The attribute of type T that exists on the enum value</returns>
		/// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
		public static T? GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute {
			Type type = enumVal.GetType();
			MemberInfo[] memInfo = type.GetMember(enumVal.ToString());
			object[] attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
			return (attributes.Length > 0) ? (T)attributes[0] : null;
		}

		private static object[] GetEnumAttributes(Type enumType) {
			return enumType.GetMembers()
				.Where(member => member.MemberType == MemberTypes.Field && ((FieldInfo)member).FieldType == enumType)
				.Select(member => member.GetCustomAttributes(false))
				.SelectMany(attributeList => attributeList)
				.ToArray();
		}
	}
}
```
</details>

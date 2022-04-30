# C# Better Enums

A source generator that adds extension methods for enums with any attribute. Also adds more performant methods.

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

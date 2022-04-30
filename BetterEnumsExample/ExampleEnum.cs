using System.ComponentModel;
using BetterEnumsGen;

namespace BetterEnumsExample;

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

[BetterEnum]
public enum AnotherEnum {
	Hello,
	[ExampleEnumInfo("Hello", 1)]
	Another,
	[Description("1")]
	One
}

public class ExampleEnumInfoAttribute : Attribute {
	public string Text { get; set; }
	public int Number { get; set; }

	public ExampleEnumInfoAttribute(string text, int number) {
		this.Text = text;
		this.Number = number;
	}
}

using BetterEnumsGen;

namespace BetterEnumsExample;

public class Example {
	public static void Main(string[] _) {
		foreach (ExampleEnum @enum in Enum.GetValues<ExampleEnum>()) {
			Console.WriteLine($"{@enum.BetterToString()}: {@enum.GetExampleEnumInfo()!.Text}/{@enum.GetExampleEnumInfo()!.Number}, Desc: {@enum.GetDescription()?.Description}");
		}

		foreach (AnotherEnum @enum in Enum.GetValues<AnotherEnum>()) {
			Console.WriteLine($"{@enum.BetterToString()}: {@enum.GetExampleEnumInfo()?.Text}/{@enum.GetExampleEnumInfo()?.Number}, Desc: {@enum.GetDescription()?.Description}");
		}
	}
}

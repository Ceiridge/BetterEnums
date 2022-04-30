using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterEnums {
	internal class EnumExtensionMethodsGenerator : IExtensionGenerator {
		private readonly EnumDeclarationSyntax enumDec;
		private readonly SemanticModel semantic;
		private readonly ISymbol symbol;

		private readonly Dictionary<string, EnumMemberDeclarationSyntax> enumMembers = new Dictionary<string, EnumMemberDeclarationSyntax>();
		private string SymbolName => $"{this.symbol.ContainingNamespace}.{this.symbol.Name}";

		private readonly string randomEnumGuid = Guid.NewGuid().ToString();

		public EnumExtensionMethodsGenerator(EnumDeclarationSyntax enumDec, SemanticModel semantic) {
			this.enumDec = enumDec;
			this.semantic = semantic;
			this.symbol = this.semantic.GetDeclaredSymbol(this.enumDec) ?? throw new KeyNotFoundException($"Symbol not found for {enumDec}");

			foreach (EnumMemberDeclarationSyntax member in this.enumDec.Members) {
				this.enumMembers.Add($"{this.SymbolName}.{member.Identifier.Text}", member);
			}
		}

		public void AddGeneration(StringBuilder builder) {
			this.AppendToString(builder);
			this.AppendValues(builder);
		}

		public void AddFieldsGeneration(StringBuilder builder) {
			int i = 0;

			foreach (INamedTypeSymbol attribute in this.GetAllAttributes()) {
				string fullType = $"{attribute.ContainingNamespace}.{attribute.Name}";
				string hash = ($"{fullType}-{this.SymbolName}-{this.randomEnumGuid}-{i++}").Sha256();

				builder.AppendLine($"private static readonly {fullType}? attr_{hash};");
			}
		}

		public void AddCctorGeneration(StringBuilder builder) {
			int i = 0;

			string guid = this.randomEnumGuid.Sha256();
			string attrsVar = $"attrs_{guid}";
			builder.AppendLine($"	object[] {attrsVar} = GetEnumAttributes(typeof({this.SymbolName}));");

			foreach (INamedTypeSymbol attribute in this.GetAllAttributes()) {
				string fullType = $"{attribute.ContainingNamespace}.{attribute.Name}";
				string hash = ($"{fullType}-{this.SymbolName}-{this.randomEnumGuid}-{i}").Sha256();

				builder.AppendLine($"	attr_{hash} = {i} >= {attrsVar}.Length ? null : {attrsVar}[{i}] as {fullType};");
				i++;
			}
		}

		/// <summary>
		/// Appends faster ToString method
		/// </summary>
		private void AppendToString(StringBuilder builder) {
			builder.Append(string.Format(BetterEnumsSources.ENUM_TO_STRING_SOURCE, this.SymbolName));
			builder.AppendLine(BetterEnumsSources.ENUM_SWITCH_SOURCE_START);

			foreach (string member in this.enumMembers.Keys) {
				builder.AppendLine($"{member} => nameof({member}),");
			}
			builder.AppendLine(BetterEnumsSources.ENUM_SWITCH_SOURCE_END);
		}

		/// <summary>
		/// Appends GetT methods where T is the attribute
		/// </summary>
		private void AppendValues(StringBuilder builder) {
			int i = 0;
			Dictionary<string, StringBuilder> methodBuilders = new Dictionary<string, StringBuilder>();

			foreach ((KeyValuePair<string, EnumMemberDeclarationSyntax> member, INamedTypeSymbol attribute) in this.GetAllAttributesFull()) {

				string fullType = $"{attribute.ContainingNamespace}.{attribute.Name}";
				string hash = ($"{fullType}-{this.SymbolName}-{this.randomEnumGuid}-{i}").Sha256();

				if (!methodBuilders.TryGetValue(fullType, out StringBuilder method)) {
					method = methodBuilders[fullType] = new StringBuilder(
						string.Format(BetterEnumsSources.ENUM_VALUES_SOURCE, fullType, attribute.Name.NoAttributeEnding(), this.SymbolName)
					).AppendLine(BetterEnumsSources.ENUM_SWITCH_SOURCE_START);
				}

				method.AppendLine($"{member.Key} => attr_{hash},");
				i++;
			}

			foreach (StringBuilder method in methodBuilders.Values) {
				method.AppendLine(BetterEnumsSources.ENUM_SWITCH_SOURCE_END_NULL);
				builder.AppendLine(method.ToString());
			}
		}

		// This is what I like about C#. Luckily I didn't have to use 4 nested for loops, although they would probably better performing somehow.
		private IEnumerable<(KeyValuePair<string, EnumMemberDeclarationSyntax> member, INamedTypeSymbol symbol)> GetAllAttributesFull() {
			return from member in this.enumMembers
				   from attributeList in member.Value.AttributeLists
				   from attribute in attributeList.Attributes
				   let symbolInfo = this.semantic.GetSymbolInfo(attribute)
				   where symbolInfo.Symbol != null
				   select (member, symbolInfo.Symbol!.ContainingType);
		}

		private IEnumerable<INamedTypeSymbol> GetAllAttributes() {
			return this.GetAllAttributesFull().Select(tuple => tuple.symbol);
		}
	}
}

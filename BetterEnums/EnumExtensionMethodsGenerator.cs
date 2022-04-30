using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Text;

namespace BetterEnums {
	internal class EnumExtensionMethodsGenerator {
		private readonly EnumDeclarationSyntax enumDec;
		private readonly SemanticModel semantic;
		private readonly ISymbol symbol;

		private readonly List<string> enumMembers = new List<string>();
		private string SymbolName => $"{this.symbol.ContainingNamespace}.{this.symbol.Name}";

		public EnumExtensionMethodsGenerator(EnumDeclarationSyntax enumDec, SemanticModel semantic) {
			this.enumDec = enumDec;
			this.semantic = semantic;
			this.symbol = this.semantic.GetDeclaredSymbol(this.enumDec) ?? throw new KeyNotFoundException($"Symbol not found for {enumDec}");

			foreach (EnumMemberDeclarationSyntax member in this.enumDec.Members) {
				this.enumMembers.Add($"{this.SymbolName}.{member.Identifier.Text}");
			}
		}

		public void AddGeneration(StringBuilder builder) {
			this.AppendToString(builder);
		}

		/// <summary>
		/// Appends faster ToString method
		/// </summary>
		private void AppendToString(StringBuilder builder) {
			builder.Append(string.Format(BetterEnumsSources.ENUM_TO_STRING_SOURCE, this.SymbolName));

			foreach (string member in this.enumMembers) {
				builder.AppendLine($"{member} => nameof({member}),");
			}
			builder.AppendLine(BetterEnumsSources.ENUM_TO_STRING_SOURCE_END);
		}
	}
}

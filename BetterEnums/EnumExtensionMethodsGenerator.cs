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
		}

		public void AddFieldsGeneration(StringBuilder builder) {
			int i = 0;

			foreach (INamedTypeSymbol attribute in this.GetAllAttributes()) {
				string fullType = $"{attribute.ContainingNamespace}.{attribute.Name}";
				string hash = ($"{fullType}-{this.SymbolName}-{this.randomEnumGuid}-{i++}").Sha256();

				builder.AppendLine($"private static readonly {fullType} attr_{hash};");
			}
		}

		public void AddCctorGeneration(StringBuilder builder) {
			int i = 0;

			foreach (INamedTypeSymbol attribute in this.GetAllAttributes()) {
				string fullType = $"{attribute.ContainingNamespace}.{attribute.Name}";
				string hash = ($"{fullType}-{this.SymbolName}-{this.randomEnumGuid}-{i++}").Sha256();

				builder.AppendLine($"	attr_{hash} = ;");
			}
		}

		/// <summary>
		/// Appends faster ToString method
		/// </summary>
		private void AppendToString(StringBuilder builder) {
			builder.Append(string.Format(BetterEnumsSources.ENUM_TO_STRING_SOURCE, this.SymbolName));

			foreach (string member in this.enumMembers.Keys) {
				builder.AppendLine($"{member} => nameof({member}),");
			}
			builder.AppendLine(BetterEnumsSources.ENUM_TO_STRING_SOURCE_END);
		}

		private IEnumerable<INamedTypeSymbol> GetAllAttributes() {
			return from attributeLists in
				this.enumMembers.Values.Select(member => member.AttributeLists)
				   from attributeList in attributeLists
				   from attribute in attributeList.Attributes
				   select this.semantic.GetSymbolInfo(attribute) into symbolInfo
				   where symbolInfo.Symbol != null
				   select symbolInfo.Symbol!.ContainingType;
		}
	}
}

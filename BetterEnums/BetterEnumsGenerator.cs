using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterEnums {
	[Generator]
	public class BetterEnumsGenerator : ISourceGenerator {
		public void Initialize(GeneratorInitializationContext context) {
		}

		public void Execute(GeneratorExecutionContext context) {
			AddAttribute(context);
			AddExtension(context, GetEnums(context));
		}

		private static void AddAttribute(GeneratorExecutionContext context) {
			context.AddSource(BetterEnumsSources.ATTRIBUTE, SourceText.From(BetterEnumsSources.ATTRIBUTE_SOURCE, Encoding.UTF8));
		}

		private static void AddExtension(GeneratorExecutionContext context, IEnumerable<EnumDeclarationSyntax> enums) {
			StringBuilder enumExtension = new StringBuilder(BetterEnumsSources.ENUM_EXTENSION_SOURCE_START);
			List<IExtensionGenerator> generators = new List<IExtensionGenerator>();

			foreach (EnumDeclarationSyntax enumDec in enums) {
				generators.Add(new EnumExtensionMethodsGenerator(enumDec,
					context.Compilation.GetSemanticModel(enumDec.SyntaxTree)));
			}


			foreach (IExtensionGenerator gen in generators) {
				gen.AddFieldsGeneration(enumExtension);
				gen.AddGeneration(enumExtension);
			}

			enumExtension.AppendLine($"static {BetterEnumsSources.EXTENSION}() {{");
			foreach (IExtensionGenerator gen in generators) {
				gen.AddCctorGeneration(enumExtension);
			}
			enumExtension.AppendLine("}");

			enumExtension.AppendLine(BetterEnumsSources.ENUM_GET_ATTRIBUTE_METHOD_SOURCE);
			enumExtension.AppendLine(BetterEnumsSources.ENUM_GET_ATTRIBUTES_METHOD_SOURCE);
			enumExtension.AppendLine(BetterEnumsSources.ENUM_EXTENSION_SOURCE_END);

			string enumExtensionSource = enumExtension.ToString();
			context.AddSource(BetterEnumsSources.EXTENSION, SourceText.From(enumExtensionSource, Encoding.UTF8));
		}

		private static IEnumerable<EnumDeclarationSyntax> GetEnums(GeneratorExecutionContext context) {
			foreach (SyntaxTree tree in context.Compilation.SyntaxTrees) {
				SemanticModel semantic = context.Compilation.GetSemanticModel(tree);

				foreach (EnumDeclarationSyntax foundEnum in tree.GetRoot().DescendantNodesAndSelf().OfType<EnumDeclarationSyntax>()) {
					ISymbol? enumSymbol = semantic.GetDeclaredSymbol(foundEnum);

					if (enumSymbol != null && enumSymbol.GetAttributes()
							.Any(attribute => attribute.AttributeClass?.Name == BetterEnumsSources.ATTRIBUTE_SHORT)) {
						yield return foundEnum;
					}
				}
			}
		}
	}
}

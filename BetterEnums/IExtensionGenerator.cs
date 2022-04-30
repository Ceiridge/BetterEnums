using System.Text;

namespace BetterEnums {
	public interface IExtensionGenerator {
		public void AddGeneration(StringBuilder builder);
		public void AddFieldsGeneration(StringBuilder builder);
		public void AddCctorGeneration(StringBuilder builder);
	}
}

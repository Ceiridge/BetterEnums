using System.Security.Cryptography;
using System.Text;

namespace BetterEnums {
	public static class StringExtensions {
		public static string Sha256(this string str) {
			using SHA256 sha256 = SHA256.Create();

			byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
			return hash.ToHex();
		}

		public static string NoAttributeEnding(this string str) {
			return str.EndsWith("Attribute")
				? str.Substring(0, str.Length - "Attribute".Length)
				: str;
		}

		public static string ToHex(this byte[] bytes, bool upperCase = false) {
			StringBuilder result = new StringBuilder(bytes.Length * 2);
			foreach (byte t in bytes) {
				result.Append(t.ToString(upperCase ? "X2" : "x2"));
			}

			return result.ToString();
		}
	}
}

namespace Termoservis.Common.Extensions
{
	/// <summary>
	/// The <see cref="string"/> extensions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Makes string searchable.
		/// </summary>
		/// <param name="string">The string.</param>
		/// <returns>Returns new instance of specified string that is searchable.</returns>
		public static string AsSearchable(this string @string)
		{
			return @string.ToLowerInvariant()
                .Replace("š", "s")
                .Replace("ć", "c")
                .Replace("č", "c")
                .Replace("đ", "d")
                .Replace("ž", "z")
                .Replace("(", " ")
                .Replace(")", " ")
                .Replace("[", " ")
                .Replace("]", " ")
                .Replace("{", " ")
                .Replace("}", " ")
                .Replace("&", " ")
                .Replace("%", " ")
                .Replace("$", " ")
                .Replace("/", " ")
                .Replace("`", " ")
                .Replace("@", " ")
                .Replace("\"", " ")
                .Replace("!", " ")
                .Replace("'", " ")
                .Replace("*", " ")
                .Replace("_", " ")
                .Replace("-", " ")
                .Replace(".", " ")
                .Replace(",", " ")
                .Trim();
		}
	}
}

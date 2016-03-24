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
			return @string.ToLowerInvariant().Trim();
		}
	}
}

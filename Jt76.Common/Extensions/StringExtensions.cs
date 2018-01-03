namespace Jt76.Common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	public static class StringExtensions
	{
		public static IEnumerable<string> SplitOnNewLines(this string strSource)
		{
			return strSource.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
		}

		public static IEnumerable<string> SplitOnBreaks(this string strSource)
		{
			return strSource.Split(new[] {"<br/>"}, StringSplitOptions.RemoveEmptyEntries);
		}

		public static string StripNewlines(this string input)
		{
			return Regex.Replace(input, "\n", "    ", RegexOptions.Compiled);
		}

		public static string DigitsOnly(this string input)
		{
			return string.IsNullOrWhiteSpace(input)
				? input
				: new string(input.Where(char.IsDigit).ToArray());
		}

		/// <summary>
		///     Tries to convert a string to in integer
		/// </summary>
		/// <param name="input">The string for this extension</param>
		/// <returns>Null value if empty or null input, or an integer of that number</returns>
		public static int ToInteger(this string input)
		{
			return string.IsNullOrWhiteSpace(input)
				? 0
				: int.Parse(input);
		}

		/// <summary>
		///     Tries to convert a string to in integer
		/// </summary>
		/// <param name="input">The string for this extension</param>
		/// <returns>Null value if empty or null input, or an integer of that number</returns>
		public static double ToDouble(this string input)
		{
			return string.IsNullOrWhiteSpace(input)
				? 0
				: double.Parse(input);
		}
	}
}
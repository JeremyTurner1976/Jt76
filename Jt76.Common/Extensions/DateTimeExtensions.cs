namespace Jt76.Common.Extensions
{
	using System;

	public static class DateTimeExtensions
	{
		public static string ToShortDateString(this DateTime dateTime)
		{
			return dateTime.ToString("g");
		}
	}
}
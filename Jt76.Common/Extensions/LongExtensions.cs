namespace Jt76.Common.Extensions
{
	using System;

	public static class LongExtensions
	{
		public static DateTime GetDateTimeFromUnixTimestamp(this long seconds)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
		}
	}
}
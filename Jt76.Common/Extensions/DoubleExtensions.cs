namespace Jt76.Common.Extensions
{
	using System;

	public static class DoubleExtensions
	{
		public static double ToPrecisionValue(this double value, int precision)
		{
			return Math.Round(value, precision);
		}
	}
}
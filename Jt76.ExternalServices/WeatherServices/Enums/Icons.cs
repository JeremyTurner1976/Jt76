namespace Jt76.ExternalServices.WeatherServices.Enums
{
	using Common.Attributes;

	//https://github.com/darkskyapp/skycons

	public enum Icons
	{
		[EnumAttributes.FileName(IconFileNames.ClearSkyDay)] ClearSkyDay,
		[EnumAttributes.FileName(IconFileNames.ClearSkyNight)] ClearSkyNight,
		[EnumAttributes.FileName(IconFileNames.PartlyCloudyDay)] PartlyCloudDay,
		[EnumAttributes.FileName(IconFileNames.PartlyCloudyNight)] PartlyCloudyNight,
		[EnumAttributes.FileName(IconFileNames.ScatteredCloudsDay)] ScatteredCloudsDay,
		[EnumAttributes.FileName(IconFileNames.ScatteredCloudsNight)] ScatteredCloudsNight,
		[EnumAttributes.FileName(IconFileNames.BrokenCloudsDay)] BrokenCloudsDay,
		[EnumAttributes.FileName(IconFileNames.BrokenCloudsNight)] BrokenCloudsNight,
		[EnumAttributes.FileName(IconFileNames.ShoweringRainDay)] ShoweringRainDay,
		[EnumAttributes.FileName(IconFileNames.ShoweringRainNight)] ShoweringRainNight,
		[EnumAttributes.FileName(IconFileNames.RainDay)] RainDay,
		[EnumAttributes.FileName(IconFileNames.RainNight)] RainNight,
		[EnumAttributes.FileName(IconFileNames.ThunderStormDay)] ThunderStormDay,
		[EnumAttributes.FileName(IconFileNames.ThunderStormNight)] ThunderStormNight,
		[EnumAttributes.FileName(IconFileNames.SnowDay)] SnowDay,
		[EnumAttributes.FileName(IconFileNames.SnowNight)] SnowNight,
		[EnumAttributes.FileName(IconFileNames.MistDay)] MistDay,
		[EnumAttributes.FileName(IconFileNames.MistNight)] MistNight
	}

	//All are png extensions
	public static class IconFileNames
	{
		public const string ClearSkyDay = "01d";
		public const string ClearSkyNight = "01n";
		public const string PartlyCloudyDay = "02d";
		public const string PartlyCloudyNight = "02n";
		public const string ScatteredCloudsDay = "03d";
		public const string ScatteredCloudsNight = "03n";
		public const string BrokenCloudsDay = "04d";
		public const string BrokenCloudsNight = "04n";
		public const string ShoweringRainDay = "09d";
		public const string ShoweringRainNight = "09n";
		public const string RainDay = "10d";
		public const string RainNight = "10n";
		public const string ThunderStormDay = "11d";
		public const string ThunderStormNight = "11n";
		public const string SnowDay = "13d";
		public const string SnowNight = "13n";
		public const string MistDay = "50d";
		public const string MistNight = "50n";
	}
}
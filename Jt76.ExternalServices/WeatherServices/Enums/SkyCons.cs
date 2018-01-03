namespace Jt76.ExternalServices.WeatherServices.Enums
{
	using Common.Attributes;

	//https://github.com/darkskyapp/skycons

	public enum SkyCons
	{
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.ClearDay)] ClearDay,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.ClearNight)] ClearNight,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.PartlyCloudyDay)] PartlyCloudyDay,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.PartlyCloudyNight)] PartlyCloudyNight,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.Cloudy)] Cloudy,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.Rain)] Rain,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.Sleet)] Sleet,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.Snow)] Snow,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.Wind)] Wind,
		[EnumAttributes.ClientSideString(SkyConClientSideStrings.Fog)] Fog
	}

	public static class SkyConClientSideStrings
	{
		public const string ClearDay = "clear-day";
		public const string ClearNight = "clear-night";
		public const string PartlyCloudyDay = "partly-cloudy-day";
		public const string PartlyCloudyNight = "partly-cloudy-night";

		public const string Cloudy = "cloudy";
		public const string Rain = "rain";
		public const string Sleet = "sleet";
		public const string Snow = "snow";
		public const string Wind = "wind";
		public const string Fog = "fog";
	}
}
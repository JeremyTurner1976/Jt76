namespace Jt76.Common.ConfigSettings.LogSettings
{
	using Enums;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public class LogSetting
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public LogType Type { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public LogLevel Level { get; set; }

		public bool On { get; set; } = false;
	}
}
namespace Jt76.Common.ConfigSettings.EmailSettings
{
	public class EmailSettings
	{
		public string EmailAddress { get; set; }
		public string EmailPassword { get; set; }
		public string DeveloperEmailAddress { get; set; }
		public string CarbobCopyEmailAddress { get; set; }
		public string BackupCarbonCopyEmailAddress { get; set; }
		public string Smtp { get; set; }
		public int Port { get; set; }
		public bool UsePickupDirectory { get; set; }
	}
}
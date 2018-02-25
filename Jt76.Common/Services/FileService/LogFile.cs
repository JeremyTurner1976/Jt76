namespace Jt76.Common.Services.FileService
{
	public class LogFile
	{
		public string FileLocation
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public string ApplicationName
		{
			get;
			set;
		}

		public string[] RecentFileLines
		{
			get;
			set;
		}

		public string[] FullFile
		{
			get;
			set;
		} = new string[0];
	}
}

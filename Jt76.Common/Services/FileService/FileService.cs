namespace Jt76.Common.Services.FileService
{
	using System;
	using System.IO;
	using System.Linq;
	using Enums;
	using Extensions;
	using Interfaces;

	//Production (TimedService): Have a service that will run on the server timed to handle file cleanup operations

	public class FileService : IFileService
	{
		private const int daysToHoldDirectoryFiles = 7;
		private const int maxDirectoryFolderFiles = 10;
		private readonly object _lock = new object();
		private readonly string executingDirectory;

		public FileService()
		{
			executingDirectory = AppContext.BaseDirectory;

			if (!Directory.Exists(GetDirectoryFolderLocation(DirectoryFolders.Email)))
				Init();
		}

		public bool SaveTextToDirectoryFile(DirectoryFolders directory, string strMessage)
		{
			string strLocationAndFile = GetDirectoryFileLocation(directory);
			lock (_lock)
			{
				File.AppendAllText(strLocationAndFile, strMessage + Environment.NewLine);
			}

			return true;
		}

		public string[] LoadTextFromDirectoryFile(DirectoryFolders directory, string strFileName = "",
			DateTime dtIdentifier = new DateTime())
		{
			string strFolderLocation = GetDirectoryFolderLocation(directory);

			string strFileAndPathName;

			if (!string.IsNullOrEmpty(strFileName))
			{
				strFileAndPathName = strFolderLocation + "\\" + strFileName;
			}
			else
			{
				string strEnum = directory.ToNameString();
				strFileAndPathName = strFolderLocation + "\\" + strEnum + "_" + dtIdentifier.ToString("M-dd-yyyy") +
				                     ".txt";
			}


			if (strFileAndPathName.Contains(".txt") && File.Exists(strFileAndPathName))
				lock (_lock)
				{
					return File.ReadAllLines(strFileAndPathName);
				}
			throw new FileNotFoundException();
		}

		public bool DeleteOldFilesInFolder(DirectoryFolders directory, int nNewestFilesToSave)
		{
			string strFolderLocation = GetDirectoryFolderLocation(directory);

			foreach (
				FileInfo file in
				new DirectoryInfo(strFolderLocation).GetFiles()
					.OrderByDescending(x => x.LastWriteTime)
					.Skip(nNewestFilesToSave))
				file.Delete();

			return true;
		}

		public bool DeleteFilesByDays(DirectoryFolders directory, int nDays)
		{
			string strFolderLocation = GetDirectoryFolderLocation(directory);

			foreach (
				FileInfo file in
				new DirectoryInfo(strFolderLocation).GetFiles()
					.Where(x => x.LastWriteTime <= DateTime.Now.AddDays(nDays * -1)))
				file.Delete();

			return true;
		}

		public string GetDirectoryFolderLocation(DirectoryFolders directory)
		{
			string strEnum = directory.ToNameString();
			return executingDirectory + "\\App_Data\\" + strEnum;
		}

		public string GetDirectoryFileName(DirectoryFolders directory, string fileName = "", bool getMostRecent = true)
		{
			string strFolderLocation = GetDirectoryFolderLocation(directory);

			if (getMostRecent)
			{
				FileInfo fileInfo = new DirectoryInfo(strFolderLocation).GetFiles()
					.OrderByDescending(x => x.CreationTime)
					.FirstOrDefault();
				return fileInfo?.FullName;
			}
			return $"{strFolderLocation}\\{fileName}";
		}

		private bool Init()
		{
			Array enumValues = Enum.GetValues(typeof(DirectoryFolders));
			foreach (object value in enumValues)
			{
				if (!Directory.Exists(GetDirectoryFolderLocation((DirectoryFolders) value)))
					Directory.CreateDirectory(GetDirectoryFolderLocation((DirectoryFolders) value));

				//Production: Service will handle this
				if ((DirectoryFolders) value != DirectoryFolders.Data)
					DeleteOldFilesInFolder((DirectoryFolders) value, maxDirectoryFolderFiles);
			}

			return true;
		}

		private string GetDirectoryFileLocation(DirectoryFolders directory)
		{
			string strEnum = directory.ToNameString();
			return GetDirectoryFolderLocation(directory) + "\\" + strEnum + "_" + DateTime.Now.ToString("yyyy-M-dd") +
			       ".txt";
		}
	}
}
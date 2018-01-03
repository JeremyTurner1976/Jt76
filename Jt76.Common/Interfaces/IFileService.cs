namespace Jt76.Common.Interfaces
{
	using System;
	using Enums;

	public interface IFileService
	{
		bool SaveTextToDirectoryFile(DirectoryFolders directory, string strMessage);

		string[] LoadTextFromDirectoryFile(
			DirectoryFolders directory,
			string strFileName = "",
			DateTime dtIdentifier = new DateTime());

		bool DeleteOldFilesInFolder(DirectoryFolders directory, int nFilesToSave);

		bool DeleteFilesByDays(DirectoryFolders directory, int nDays);

		string GetDirectoryFolderLocation(DirectoryFolders directory);

		string GetDirectoryFileName(DirectoryFolders directory, string fileName = "", bool getMostRecent = true);
	}
}
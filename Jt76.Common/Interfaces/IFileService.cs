namespace Jt76.Common.Interfaces
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using Enums;

	public interface IFileService
	{
		bool SaveTextToDirectoryFile(
			DirectoryFolders directory, 
			string strMessage);

		string[] LoadTextFromDirectoryFile(
			DirectoryFolders directory,
			string strFileName = "",
			int? trailingLineCount = null,
			DateTime dtIdentifier = new DateTime());

		string[] LoadTextFromFile(
			string fileLocation,
			string fileName, 
			int? trailingLineCount = null);

		bool DeleteOldFilesInFolder(
			DirectoryFolders directory, 
			int nFilesToSave);

		bool DeleteFilesByDays(
			DirectoryFolders directory, 
			int nDays);

		string GetDirectoryFileName(
			DirectoryFolders directory,
			string fileName = "",
			bool getMostRecent = true);

		string GetDirectoryFolderLocation(
			DirectoryFolders directory, 
			string sharedApplicationLocation = null);

		IEnumerable<FileInfo> GetDirectoryFiles(
			DirectoryFolders directory, 
			string sharedApplicationLocation = null);

	}
}
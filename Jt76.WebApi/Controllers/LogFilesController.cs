namespace Jt76.WebApi.Controllers
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net.NetworkInformation;
	using Common.Enums;
	using Common.Interfaces;
	using Common.Services.FileService;
	using Data;
	using Data.Factories;
	using Data.Models;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/v1/[controller]")]
	public class LogFilesController : Controller
	{
		private readonly IFileService fileService;

		public LogFilesController(
			IFileService _fileService)
		{
			fileService = _fileService;
		}

		[HttpGet]
		public IEnumerable<object> Get()
		{
			//TODO union with UI files, and handle those file locations in Gets
			return fileService
				.GetDirectoryFiles(DirectoryFolders.Errors)
				.Select(
					fileInfo =>
					new
						{
							FileLocation = fileInfo.DirectoryName,
							FileName = fileInfo.Name,
						});
		}

		[HttpGet("GetFileLines")]
		public string[] GetFileLines(
			string fileLocation,
			string fileName)
		{
			return fileService.LoadTextFromDirectoryFile(
				DirectoryFolders.Errors,
				fileName);
		}

		[HttpGet("GetLastFileLines")]
		public string[] GetLastFileLines(
			string fileLocation,
			string fileName,
			int count)
		{
			return fileService.LoadTextFromDirectoryFile(
				DirectoryFolders.Errors,
				fileName,
				count);
		}
	}
}
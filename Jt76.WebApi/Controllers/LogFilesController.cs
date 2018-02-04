namespace Jt76.WebApi.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using Common.Enums;
	using Common.Interfaces;
	using Common.Services.FileService;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;

	[Route("api/v1/[controller]")]
	public class LogFilesController : Controller
	{
		private readonly IFileService fileService;
		private readonly IConfiguration configuration;

		public LogFilesController(
			IFileService _fileService,
			IConfiguration _configuration)
		{
			fileService = _fileService;
			configuration = _configuration;
		}

		[HttpGet]
		public IEnumerable<LogFile> Get()
		{
			IEnumerable<LogFile> databaseFiles = fileService
				.GetDirectoryFiles(DirectoryFolders.Errors)
				.Select(
					fileInfo =>
						new LogFile
						{
							FileLocation = fileInfo.DirectoryName,
							FileName = fileInfo.Name,
							ApplicationName = "Database"
						});
			IEnumerable<LogFile> uiFiles = fileService
				.GetDirectoryFiles(
					DirectoryFolders.Errors,
					configuration.GetValue<string>("SharedApplicationLocation"))
				.Select(
					fileInfo =>
						new LogFile
						{
							FileLocation = fileInfo.DirectoryName,
							FileName = fileInfo.Name,
							ApplicationName = "UI"
						});

			return databaseFiles.Union(uiFiles)
				.OrderByDescending(file => file.FileName);
		}

		[HttpGet("GetFileLines")]
		public string[] GetFileLines(
			string fileLocation,
			string fileName)
		{
			return fileService.LoadTextFromFile(
				fileLocation,
				fileName);
		}

		[HttpGet("GetLastFileLines")]
		public string[] GetLastFileLines(
			string fileLocation,
			string fileName,
			int count)
		{
			return fileService.LoadTextFromFile(
				fileLocation,
				fileName,
				count);
		}
	}
}
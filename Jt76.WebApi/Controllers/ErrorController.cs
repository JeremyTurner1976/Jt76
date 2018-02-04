namespace Jt76.WebApi.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using Data;
	using Data.Factories;
	using Data.Models;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/v1/[controller]")]
	public class ErrorController : Controller
	{
		private readonly UnitOfWork applicationData;

		public ErrorController(
			UnitOfWork applicationData)
		{
			this.applicationData = applicationData;
		}

		[HttpGet]
		public IEnumerable<object> Get()
		{
			return applicationData.Errors
				.GetAll()
				.OrderByDescending(error => error.CreatedDate);
		}

		[HttpGet("{id}")]
		public Error Get(int id)
		{
			return applicationData.Errors
				.GetSingleOrDefault(x => x.Id == id);
		}

		[HttpGet("GetAsHtml/{id}")]
		public object GetAsHtml(int id)
		{
			return new
			{
				html = 
					applicationData.Errors
					.GetSingleOrDefault(x => x.Id == id)
					.ToHtml()
			};
		}

		[HttpPost]
		public IActionResult Post([FromBody] Error value)
		{
			applicationData.Errors.Update(value);
			applicationData.SaveChanges();
			return Ok();
		}

		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] Error value)
		{
			applicationData.Errors.Add(value);
			applicationData.SaveChanges();
			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id, [FromBody] Error value)
		{
			applicationData.Errors.Remove(value);
			applicationData.SaveChanges();
			return Ok();
		}
	}
}
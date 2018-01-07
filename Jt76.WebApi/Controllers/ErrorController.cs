namespace Jt76.WebApi.Controllers
{
	using System.Collections.Generic;
	using Data;
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

		// GET api/values
		[HttpGet]
		public IEnumerable<Error> Get()
		{
			return applicationData.Errors.GetAll();
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public Error Get(int id)
		{
			return applicationData.Errors.GetSingleOrDefault(x => x.Id == id);
		}

		// POST api/values
		[HttpPost]
		public IActionResult Post([FromBody] Error value)
		{
			applicationData.Errors.Update(value);
			applicationData.SaveChanges();
			return Ok();
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] Error value)
		{
			applicationData.Errors.Add(value);
			applicationData.SaveChanges();
			return Ok();
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id, [FromBody] Error value)
		{
			applicationData.Errors.Remove(value);
			applicationData.SaveChanges();
			return Ok();
		}
	}
}
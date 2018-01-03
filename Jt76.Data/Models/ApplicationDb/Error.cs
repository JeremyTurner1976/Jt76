// NOTE: This error model is in 2 places, 
// please duplicate in Common/CommonModels as the DB entities are created from that location

namespace Jt76.Data.Models.ApplicationDb
{
	using System;
	using Abstract;
	using Interfaces;

	public class Error : ModelBase, IAuditableEntity
	{
		public string Message { get; set; }

		public string ErrorLevel { get; set; }

		public string Source { get; set; }

		public string AdditionalInformation { get; set; }

		public string StackTrace { get; set; }

		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}
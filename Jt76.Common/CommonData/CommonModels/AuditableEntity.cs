namespace Jt76.Common.CommonData.CommonModels
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Interfaces;

	public class AuditableEntity : IAuditableEntity
	{
		[MaxLength(256)]
		public string CreatedBy { get; set; }

		[MaxLength(256)]
		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
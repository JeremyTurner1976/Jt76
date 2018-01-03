namespace Jt76.Common.Services.EmailService.Models
{
	using System.Collections.Generic;

	public class Email
	{
		public List<string> ToAddresses { get; set;  }
		public List<string> CarbonCopies { get; set;  }
		public List<string> BackupCarbonCopies { get; set;  }
		public string Message { get; set;  }
		public string Body { get; set; }
	}
}

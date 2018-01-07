namespace Jt76.Identity.Models.MappedModels
{
	using System.ComponentModel.DataAnnotations;

	public class UserEdit : User
	{
		public string CurrentPassword { get; set; }

		[MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
		public string NewPassword { get; set; }

		private new bool IsLockedOut { get; } //Hide base member
	}
}